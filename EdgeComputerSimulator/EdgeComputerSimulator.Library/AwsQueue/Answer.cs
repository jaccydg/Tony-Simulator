using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.AwsQueue
{
    public class Answer
    {
        public RequestType RequestType { get; private set; }
        public BoolAnswer BoolAnswer { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ColumnId { get; private set; }
        public Guid GatewayId { get; private set; }

        public Answer(Request request, BoolAnswer answer)
        {
            BoolAnswer = answer;

            RequestType = request.RequestType;
            UserId = request.UserId;
            ColumnId = request.ColumnId;
            GatewayId = request.GatewayId;
        }

    }
}
