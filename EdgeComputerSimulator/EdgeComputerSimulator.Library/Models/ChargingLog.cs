using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Models
{
    public class ChargingLog
    {
        Guid Id { get; } = Guid.NewGuid();
        public required decimal ChargingSpeed { get; init; } // kW
        public required TimeSpan ChargingTime { get; init; }
        public required decimal ConsuptionSoFar { get; init; } // kWh
        public required decimal CostSoFar { get; init; }
        public required Guid UserId { get; init; }
        public required Guid ColumnId { get; init; }


    }
}
