using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EdgeComputerSimulator.Library.Models
{
    public class Gateway
    {
        public Guid Id { get; private set; }

        private IList<Column> _columns;
        public IList<Column> Columns
        {
            get => _columns;
            set
            {
                //for (int i = 0; i < value.Count - 1; i++)
                //{
                //    value[i].Number = i + 1;
                //}
                //_columns = value;
            }
        }
        public string Code { get; private set; } = string.Empty;
        public DataForLogRandomization DataLogRnd { get; private set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }



        public Gateway(IList<Column> columns, DataForLogRandomization dataLogRnd, string code, Guid id, double latitude, double longitude)
        {
            Columns = columns;
            DataLogRnd = dataLogRnd;
            Code = code;
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void StartChargingAColumn(Guid colId)
        {

            // Find the column with the specified colId
            Column? columnToCharge = Columns.FirstOrDefault(col => col.Id.Equals(colId));

            if (columnToCharge is not null)
            {
                // Call the StartCharging method of the column
                columnToCharge.StartCharging(DataLogRnd, Id, Code);
            }
            else
            {
                // Handle case where column with colId is not found
                throw new ArgumentException($"Column with ID {colId} not found.");
            }

        }

        public void SendLogAnalytics()
        {

            // TODO --> Send logs of the gateway:
            // 1. Station total consumption.
            // 2. Occupation percentage.

        }

    }
    public record DataForLogRandomization
    {
        public required EVChargerLevel EVChargerLevelOfColumns { get; init; }
        public required TimeSpan LogIntervalSendingTime { get; init; }

    }
}
