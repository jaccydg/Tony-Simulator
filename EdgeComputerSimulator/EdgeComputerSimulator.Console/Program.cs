
//// Here I will put all of the code which retrieves the stations and columns from the database and
//// simulates the logs to send. 
//// So also the part of the logs sent in the AWS queue.

//using EdgeComputerSimulator.Library.Enums;
//using EdgeComputerSimulator.Library.Models;
//using Amazon;
//using Amazon.SQS;
//using Amazon.SQS.Model;
//using Amazon.Runtime;
//using EdgeComputerSimulator.Library.AwsQueue;
//using System.Net.Http.Json;
//using EdgeComputerSimulator.Library.Dtos;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using Newtonsoft.Json;
//using EdgeComputerSimulator.Library;
//using Spectre.Console;



////gateway.StartChargingAColumn(gateway.Columns.First().Id);
////Console.ReadLine();



//public class Program
//{
//    private static readonly HttpClient client = new HttpClient();

//    public static async Task Main(string[] args)
//    {

//        try
//        {
//            //var chargingStations = await GetChargingStationsAsync("https://tonyapi.ddns.net/ChargingStations");
//            var chargingStations = await GetChargingStationsAsync("http://localhost:3000/ChargingStations");
//            var gateways = await GetGatewaysAsync("http://localhost:3000/Gateways");
//            //var gateways = await GetGatewaysAsync("https://tonyapi.ddns.net/Gateways");

//            foreach (var station in chargingStations)
//            {
//                Console.WriteLine($"ID STATION: {station.Id}");
//            }
//            foreach (var gateway in gateways)
//            {
//                Console.WriteLine($"ID GATEWAY: {gateway.Id}");
//            }


//            GatewaysCollectionInitialization(gateways, chargingStations);

//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"An error occurred: {ex.Message}");
//        }

//    }



//    public static async Task<List<ColumnListDto>> GetChargingStationsAsync(string url)
//    {

//        var response = await client.GetStringAsync(url);
//        if (response is null) return new();

//        List<ColumnListDto> columns = JsonConvert.DeserializeObject<List<ColumnListDto>>(response);

//        return columns ?? new();
    
//    }
    
//    public static async Task<List<GatewayListDto>> GetGatewaysAsync(string url)
//    {

//        var response = await client.GetStringAsync(url);
//        if (response is null) return new();

//        List<GatewayListDto> gateways = JsonConvert.DeserializeObject<List<GatewayListDto>>(response);

//        return gateways ?? new();
//    }

    
//    public static void GatewaysCollectionInitialization(List<GatewayListDto> gateways, List<ColumnListDto> columns)
//    {
//        if (gateways is null)
//        {
//            Console.WriteLine("No gateways found in the db.");
//            return;
//        }

//        foreach (var gateway in gateways)
//        {
//            var correspondingColumns = FindCorrespondingColumnsOfGateway(columns, gateway);

//            var correspondingMappedColumns = MapDtoColumnsToMyColumns(correspondingColumns);


//            EVChargerLevel evcl = new EVChargerLevel(EVCLevel.Level2);
//            DataForLogRandomization dflr = new() 
//            {
//                EVChargerLevelOfColumns = evcl, 
//                LogIntervalSendingTime = TimeSpan.FromSeconds(2) 
//            };

//            GatewaysCollection.Gateways.Add(new Gateway(correspondingMappedColumns, dflr, gateway.Name, gateway.Id, gateway.Latitude, gateway.Longitude));

//            int a = 0;
//        }

//    }

//    private static List<ColumnListDto> FindCorrespondingColumnsOfGateway(List<ColumnListDto> columns, GatewayListDto gateway)
//    {
//        return columns.FindAll(col => col.GatewayId == gateway.Id);
        
//    }

//    private static List<Column> MapDtoColumnsToMyColumns(List<ColumnListDto> dtoColumns) 
//    {
//        List<Column> mappedColumns = new();

//        foreach (var col in dtoColumns)
//        {
//            mappedColumns.Add(new()
//            {
//                Id = col.Id,
//                Number = col.Number,
//                Status = col.Status,
//            });
//        }
//        return mappedColumns;
//    }

//}