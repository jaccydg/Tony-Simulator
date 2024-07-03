using EdgeComputerSimulator.Library.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Console.RNDlogs.Json
{
    public class ColumnListDtoContainer
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; }

        [JsonPropertyName("$values")]
        public List<ColumnListDto> Values { get; set; }
    }
}
