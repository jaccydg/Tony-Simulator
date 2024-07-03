
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
using System.Net.Http.Json;
using EdgeComputerSimulator.Library.Dtos;
using EdgeComputerSimulator.Console.RNDlogs;
using System.Text.Json;
using EdgeComputerSimulator.Console.RNDlogs.Json;
using System.Text.Json.Serialization;



//gateway.StartChargingAColumn(gateway.Columns.First().Id);
//Console.ReadLine();



public class Program
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task Main(string[] args)
    {
        try
        {
            var chargingStations = await GetChargingStationsAsync("http://localhost:3000/ChargingStations");
            foreach (var station in chargingStations)
            {
                Console.WriteLine($"ID: {station.Id}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static async Task<List<ColumnListDto>> GetChargingStationsAsync(string url)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        var response = await client.GetStringAsync(url);
        var container = JsonSerializer.Deserialize<ColumnListDtoContainer>(response, options);

        //TODO
        return new List<ColumnListDto>();
    }

}