using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Models
{
    public class Gateway
    {
        public Guid Id { get; } = Guid.NewGuid();
        public required IList<Column> Columns { get; init; }
        public string Code { get; init; } = string.Empty;
        public required DataForLogRandomization DataLogRnd { get; init; }


        //public double Latitude { get; init; }
        //public double Longitude { get; init; }


        public void GenerateNewRandomizedLogDataForColumns()
        {
            foreach (var col in Columns)
            {
                col.RandomizeLogToSend(DataLogRnd);
            }
        }

        public void SendLogsFromEachColumn()
        {

            foreach(var col in Columns)
            {
                col.SendLog();
            }

        }

        public record DataForLogRandomization
        {
            public required EVChargerLevel EVChargerLevelOfColumns { get; init; }
            public required TimeSpan LogIntervalSendingTime { get; init; }

        }

    }
}
