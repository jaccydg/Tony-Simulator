// 1. Ask for gateways and columns

using EdgeComputerSimulator.Library.Enums;
using EdgeComputerSimulator.Library.Models;

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

gateway.StartChargingAColumn(gateway.Columns.First().Id);


Console.ReadLine();
Console.ReadLine();
