using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using EdgeComputerSimulator.Library.AwsQueue;

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

string queueUrl = "https://sqs.eu-west-1.amazonaws.com/240595528763/clod-digregorio-projectwork-2.fifo";

var credentials = AwsQueueConnector.LoadAWSCredentials();
var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1); // Change region if needed

Console.WriteLine("Listening for messages...");

while (true)
{
    await PollQueue(sqsClient);
    await Task.Delay(5000); // Poll every 5 seconds
}


async Task PollQueue(IAmazonSQS sqsClient)
{
    var receiveMessageRequest = new ReceiveMessageRequest
    {
        QueueUrl = queueUrl,
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

    var requestMessage = JsonSerializer.Deserialize<Request>(message.Body);

    if(requestMessage is null)
        { return; }

    // TODO --> Check the column of the request status.
    // If status is free --> send in the answers queue a positive answer.
    // Otherwise --> send in the answers queue a negative answer.

    // Delete the message after processing
    await DeleteMessage(sqsClient, message.ReceiptHandle);
}

async Task DeleteMessage(IAmazonSQS sqsClient, string receiptHandle)
{
    var deleteMessageRequest = new DeleteMessageRequest
    {
        QueueUrl = queueUrl,
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


