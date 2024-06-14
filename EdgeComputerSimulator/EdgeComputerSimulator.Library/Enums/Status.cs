using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Enums
{
    public enum Status
    {
        Free, // The column is free
        Charging, // A user is connected and the column is charging.
        Completed, // A user finished his charge and disconnected from the column.
        Idle // A user finished charging but didn't disconnect from the column yet.
    }

}
