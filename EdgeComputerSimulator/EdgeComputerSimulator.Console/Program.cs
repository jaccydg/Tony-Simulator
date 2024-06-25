
using EdgeComputerSimulator.Library.Enums;
using EdgeComputerSimulator.Library.Models;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;

var columns = new List<Column>
            {
                new() {Status = ChargingStationStatus.Charging},
                new() {Status = ChargingStationStatus.Free},
                new() {Status = ChargingStationStatus.Free}
            };

columns.First().ConnectUser(new User() { Sub = Subscription.Basic });

// Initialize DataForLogRandomization object
var dataLogRandomization = new DataForLogRandomization
{
    EVChargerLevelOfColumns = new EVChargerLevel(EVCLevel.Level2),
    LogIntervalSendingTime = TimeSpan.FromSeconds(2)
};

// Create an instance of the Gateway class
var gateway = new Gateway
(
    columns,
    dataLogRandomization,
    code: "T391G"
);

//gateway.StartChargingAColumn(gateway.Columns.First().Id);
//Console.ReadLine();


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

var credentials = LoadAWSCredentials();
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
    // Process the message
    Console.WriteLine($"Processing message: {message.Body}");

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


static AWSCredentials LoadAWSCredentials()
{
    var credentialsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aws", "credentials");
    var lines = File.ReadAllLines(credentialsFilePath);

    var profileName = "default";
    var accessKeyId = string.Empty;
    var secretAccessKey = string.Empty;

    var isInDefaultProfile = false;
    foreach (var line in lines)
    {
        if (line.Trim().Equals($"[{profileName}]", StringComparison.OrdinalIgnoreCase))
        {
            isInDefaultProfile = true;
            continue;
        }

        if (isInDefaultProfile)
        {
            if (line.Trim().StartsWith("["))
            {
                break; // End of default profile section
            }

            var keyValue = line.Split(new[] { '=' }, 2);
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();
                if (key.Equals("aws_access_key_id", StringComparison.OrdinalIgnoreCase))
                {
                    accessKeyId = value;
                }
                else if (key.Equals("aws_secret_access_key", StringComparison.OrdinalIgnoreCase))
                {
                    secretAccessKey = value;
                }
            }
        }
    }

    if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(secretAccessKey))
    {
        throw new InvalidOperationException("AWS credentials not found in the specified profile.");
    }

    return new BasicAWSCredentials(accessKeyId, secretAccessKey);
}