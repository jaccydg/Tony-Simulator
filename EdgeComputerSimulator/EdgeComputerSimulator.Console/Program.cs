
// Here I will put all of the code which retrieves the stations and columns from the database and
// simulates the logs to send. 
// So also the part of the logs sent in the AWS queue.

using EdgeComputerSimulator.Library.Enums;
using EdgeComputerSimulator.Library.Models;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;
using EdgeComputerSimulator.Library.AwsQueue;

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
var credentials = AwsQueueConnector.LoadAWSCredentials();

var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1);

// TODO --> Here I've to make a script to send the random logs in the queue.