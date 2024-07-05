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
        public RequestType RequestType { get; set; }
        public Guid UserId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid GatewayId { get; set; }

    }
}
