using EdgeComputerSimulator.Library.AwsQueue;
using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Console.RequestsHandler.Helpers
{
    public static class Deserializer
    {
        public static Request Deserialize(string message)
        {
            var request = new Request();

            var pairs = message.Trim('{', '}', ' ').Split(',');

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=').Select(p => p.Trim()).ToArray();
                var key = keyValue[0];
                var value = keyValue[1];

                switch (key)
                {
                    case "RequestType":
                        request.RequestType = Enum.Parse<RequestType>(value);
                        break;
                    case "UserId":
                        request.UserId = Guid.Parse(value);
                        break;
                    case "ChargingStationId":
                        request.ColumnId = Guid.Parse(value);
                        break;
                    case "GatewayId":
                        request.GatewayId = Guid.Parse(value);
                        break;
                }
            }

            return request;
        }
    }
}
