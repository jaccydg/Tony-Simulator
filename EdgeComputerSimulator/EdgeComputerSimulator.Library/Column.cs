using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library
{
    public enum Status
    {
        Free, // The column is free
        Charging, // A user is connected and the column is charging.
        Completed, // A user finished his charge and disconnected from the column.
        Idle // A user finished charging but didn't disconnect from the column yet.
    }
    public enum EVChargerLevel
    {
        Level1 = 1, // Speed: 1 - 1.8 kW    Estimated time: 22 - 40 hours 
        Level2 = 2, // Speed: 3 - 22 kW    Estimated time: 2 - 13 hours
        Level3 = 3  // Speed: 30 - 360 kW    Estimated time: 15 min - 1.5 hours
    }


    public class Column
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Status Status
        {
            get => Status;
            set
            {
                Status = value;
                if (value.Equals(Status.Completed))
                {
                    LastLogOfCurrentCharge = null;
                }
            }
        }
        public Guid ConnectedUserId { get; set; }
        public int Number { get; init; }
        /// <summary>
        /// It's set back to null every time 
        /// </summary>
        public ChargingLog? LastLogOfCurrentCharge { get; set; } = null;

        /// <summary>
        /// Starts being null, it's initialized when the method RandomizeLogData is called and then it is set back
        /// to null when the method SendLogsFromEachColumn() of Gateway obj is called.
        /// </summary>
        public ChargingLog? LogToSend { get; private set; } = null; 



        public void RandomizeLogToSend(EVChargerLevel EVClevel)
        {

            switch (EVClevel)
            {
                case EVChargerLevel.Level1:
                    
                    break;

                case EVChargerLevel.Level2:
                    break;

                case EVChargerLevel.Level3:
                    break;
            }

            if (LastLogOfCurrentCharge is null)
            {
                RandomizeLogDataNotGivenLastLog();
            }
            else
            {
                RandomizeLogDataGivenLastLog();
            }
        }

        private void RandomizeLogDataGivenLastLog()
        {

        }

        private void RandomizeLogDataNotGivenLastLog()
        {

        }

    }


}
