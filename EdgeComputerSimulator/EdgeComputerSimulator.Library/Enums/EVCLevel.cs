using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Enums
{
    public enum EVCLevel // Electric Vehicle Charger Level
    {
        Level1 = 1, // Speed: 1 - 1.8 kW    Estimated time: 22 - 40 hours 
        Level2 = 2, // Speed: 3 - 22 kW    Estimated time: 2 - 13 hours
        Level3 = 3  // Speed: 30 - 360 kW    Estimated time: 15 min - 1.5 hours
    }
}
