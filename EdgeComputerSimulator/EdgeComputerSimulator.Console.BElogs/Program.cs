using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using EdgeComputerSimulator.Library.AwsQueue;
using EdgeComputerSimulator.Library.Enums;
using EdgeComputerSimulator.Library;
using System.Text.RegularExpressions;
using EdgeComputerSimulator.Console.RequestsHandler.Helpers;


await GatewaysRetriever.Execute();

// In this program there will only be the retriving of requests from the AWS queue
// It will handle the requests answering by following some logics.

// REQUESTS HANDLING

// Connection request data:
// request : connection --> To know that it's a connection request.
// IdUtente : Guid
// IdColonnina : int
// IdGateway : int

// Request acceptance logic:
// Send Accepted in the queue --> If the status of the column is Free.
// Send Refused in the queue --> With any other column status.


string queueAnswerUrl = "https://sqs.eu-west-1.amazonaws.com/240595528763/clod-pw-g2-tony-queue-2.fifo";
string queueRequestUrl = "https://sqs.eu-west-1.amazonaws.com/240595528763/clod-pw-g2-tony-queue-1.fifo";

var credentials = AwsQueueConnector.LoadAWSCredentials();
var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1); // Change region if needed

Console.WriteLine("Listening for messages...");

while (true)
{
    await Task.Delay(5000); // Poll every 5 seconds
    await PollQueue(sqsClient);
}


async Task PollQueue(IAmazonSQS sqsClient)
{
    var receiveMessageRequest = new ReceiveMessageRequest
    {
        QueueUrl = queueRequestUrl,
        MaxNumberOfMessages = 10,
        WaitTimeSeconds = 10,
        VisibilityTimeout = 30
    };

    var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

    if (receiveMessageResponse.Messages.Count > 0)
    {
        foreach (var message in receiveMessageResponse.Messages)
        {
            Console.WriteLine($"Received message: {message.Body}");
            await ProcessMessage(sqsClient, message);
        }
    }
}


async Task ProcessMessage(IAmazonSQS sqsClient, Message message)
{
    Console.WriteLine($"Received message: {message.Body}");
    //var messageBody = ChangeFirstAndLastToQuotes(message.Body);

    Request requestMessage = Deserializer.Deserialize(message.Body);

    if (requestMessage is null)
    { return; }

    // TODO --> Check the column of the request status.
    // If status is free --> send in the answers queue a positive answer.
    // Otherwise --> send in the answers queue a negative answer.

    switch (requestMessage.RequestType)
    {
        case RequestType.Connection:
            {
                var column = GatewaysCollection.Gateways.Find(g => g.Id == requestMessage.GatewayId)
                    .Columns.ToList().Find(c => c.Id == requestMessage.ColumnId);

                if (column is null)
                {
                    Console.WriteLine("The column doesn't exist.");
                    return;
                }

                var answer = new Answer(requestMessage, BoolAnswer.Refused);

                if (column.Status == ChargingStationStatus.Free)
                {
                    answer = new Answer(requestMessage, BoolAnswer.Accepted);
                }

                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = queueAnswerUrl,
                    MessageBody = answer.ToString(),
                    MessageGroupId = "groupId",
                    MessageDeduplicationId = Guid.NewGuid().ToString()
                };
                var sendMessageResponse = await sqsClient.SendMessageAsync(sendMessageRequest);
                break;
            }
        case RequestType.Charge:
            {

                break;
            }
    }

    // Delete the message after processing
    await DeleteMessage(sqsClient, message.ReceiptHandle);
}

async Task DeleteMessage(IAmazonSQS sqsClient, string receiptHandle)
{
    var deleteMessageRequest = new DeleteMessageRequest
    {
        QueueUrl = queueRequestUrl,
        ReceiptHandle = receiptHandle
    };

    var deleteMessageResponse = await sqsClient.DeleteMessageAsync(deleteMessageRequest);

    if (deleteMessageResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
    {
        Console.WriteLine("Message deleted successfully.");
    }
    else
    {
        Console.WriteLine("Failed to delete message.");
    }
}

static string ChangeFirstAndLastToQuotes(string input)
{
    if (input.Length < 2)
    {
        throw new ArgumentException("Input string must have at least two characters.");
    }

    char firstChar = '"';
    char lastChar = '"';

    // Replace the first and last characters with double quotes
    string result = firstChar + input.Substring(1, input.Length - 2) + lastChar;

    return result;
}

