using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace EdgeComputerSimulator.Console.RNDlogs.Json
{
    public class Root
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("$values")]
        public List<Value> Values { get; set; }
    }

    public class Value
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("id")]
        public string IdValue { get; set; }

        public int Number { get; set; }
        public int Status { get; set; }
        public string UserConnectedId { get; set; }
        public string GatewayId { get; set; }
    }
}
