using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library
{
    public class Gateway
    {
        public Guid Id { get; } = Guid.NewGuid();
        public required IList<Column> Columns { get; init; }
        public string Code { get; init; } = string.Empty;
        public EVChargerLevel EVChargerLevelOfColumns { get; init; }

        //public double Latitude { get; init; }
        //public double Longitude { get; init; }


        public void GenerateNewRandomizedLogDataForColumns()
        {
            foreach (var col in Columns)
            {
                col.RandomizeLogToSend(EVChargerLevelOfColumns);
            }
        }

        public void SendLogsFromEachColumn()
        {



        }

    }
}
