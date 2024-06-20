using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Enums
{
    public enum ChargingStationStatus // Charging Station = Column
    {
        Free, // The column is free
        Charging, // A user is connected and the column is charging.
        Completed, // It's only for the last log of a charging session.
        Idle // A user is connected to the column but the car is not getting charged yet.
    }

}
