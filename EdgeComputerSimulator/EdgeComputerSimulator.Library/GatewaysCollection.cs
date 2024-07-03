using EdgeComputerSimulator.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library
{
    /// <summary>
    /// This class contains all of the Gateways saved in the database with the relative columns.
    /// </summary>
    public static class GatewaysCollection
    {
        public static List<Gateway> Gateways { get; set; } = new List<Gateway>();
    }
}
