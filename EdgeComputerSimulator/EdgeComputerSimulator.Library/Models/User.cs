using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Models
{
    public class User
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required Subscription Sub { get; init; }
    }
}
