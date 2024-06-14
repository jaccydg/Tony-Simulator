using EdgeComputerSimulator.Library.Enums;
using static EdgeComputerSimulator.Library.Models.Gateway;

namespace EdgeComputerSimulator.Library.Models
{
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
        public User? UserConnectedToTheColumn { get; set; } = null;


        public void RandomizeLogToSend(DataForLogRandomization dataLogRnd)
        {
            if (UserConnectedToTheColumn is not null)
            {
                decimal chargingSpeed = RandomizeSpeedLog(dataLogRnd.EVChargerLevelOfColumns, UserConnectedToTheColumn);
                TimeSpan chargingTime = dataLogRnd.LogIntervalSendingTime;

                if (LastLogOfCurrentCharge is not null)
                {
                    chargingTime = CalculateChargingTimeFromLastLog(dataLogRnd.LogIntervalSendingTime);
                }

                // The chargingSpeed is kW so I divide the chargingTime (seconds) for 3600 seconds to find the
                // consumption which is in kWh.
                decimal consumptionSoFar = chargingSpeed * (decimal)(chargingTime.TotalSeconds / 3600);

                decimal costSoFar = consumptionSoFar * 0.11m; // TODO --> Parameterize the 0.11 which indicates €/kWh. 

                LogToSend = new()
                {
                    ChargingSpeed = chargingSpeed,
                    CostSoFar = costSoFar,
                    ChargingTime = chargingTime,
                    ColumnId = Id,
                    ConsuptionSoFar = consumptionSoFar,
                    UserId = UserConnectedToTheColumn.Id
                };
            }
            else
            {
                // TODO --> Think if there's something to randomize for logs in the case there's no user connected to the column.
            }

        }

        public void SendLog()
        {
            if (LogToSend is not null)
            {
                // TODO --> Send log into a queue for the Display console to receive it.

                LogToSend = null;
            }
            else
            {
                throw new Exception("LogToSend is null");
            }
        }


        private TimeSpan CalculateChargingTimeFromLastLog(TimeSpan logIntervalSendingTime)
        {
            return LastLogOfCurrentCharge.ChargingTime + logIntervalSendingTime;
        }

        private decimal RandomizeSpeedLog(EVChargerLevel EVClevel, User user)
        {

            Random rnd = new Random();
            decimal chargingSpeed = EVClevel.MinSpeed + (decimal)rnd.NextDouble() * (EVClevel.MaxSpeed - EVClevel.MinSpeed);

            // TODO --> User subscription
            
            return chargingSpeed;

        }


        public record LogDataDependingOnPreviowsLog
        {
            public decimal ConsuptionSoFar { get; init; }
            public TimeSpan LastName { get; init; }
        }

    }


}
