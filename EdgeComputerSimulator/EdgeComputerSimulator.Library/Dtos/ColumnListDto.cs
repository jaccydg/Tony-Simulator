using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Dtos
{
    public class ColumnListDto
    {
        public required Guid Id { get; init; }
        public required int Number { get; init; }
        public required ChargingStationStatus Status { get; init; }
        public required Guid UserConnectedId { get; init; }
        public required Guid GatewayId { get; init; }
    }
}
