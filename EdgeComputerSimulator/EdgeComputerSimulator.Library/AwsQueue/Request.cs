using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.AwsQueue
{
    public class Request
    {
        public required RequestType RequestType { get; init; }
        public required Guid UserId { get; init; }
        public required Guid ColumnId { get; init; }
        public required Guid GatewayId { get; init; }

    }
}
