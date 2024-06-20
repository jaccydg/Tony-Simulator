using EdgeComputerSimulator.Library.Enums;
using System.Drawing;
using static EdgeComputerSimulator.Library.Models.Gateway;
using static System.Net.Mime.MediaTypeNames;
using Spectre.Console;

namespace EdgeComputerSimulator.Library.Models
{
    public class Column
    {
        public Guid Id { get; } = Guid.NewGuid();

        private ChargingStationStatus _status;
        public required ChargingStationStatus Status
        {
            get => _status;
            set
            {
                if (!Status.Equals(ChargingStationStatus.Free) && value.Equals(ChargingStationStatus.Free))
                {
                    LastLogOfCurrentCharge = null;
                }
                _status = value;
            }
        }
        public int Number { get; set; } // It's set in the gateway because it depends on how many other columns the gateway already has.
        /// <summary>
        /// It's set back to null every time a new user attaches.
        /// </summary>
        public ChargingLog? LastLogOfCurrentCharge { get; set; } = null;

        /// <summary>
        /// Starts being null, it's initialized when the method RandomizeLogData is called and then it is set back
        /// to null when the method SendLogsFromEachColumn() of Gateway obj is called.
        /// </summary>
        public ChargingLog? LogToSend { get; private set; } = null;
        public User? ConnectedUser { get; private set; } = null;


        private System.Timers.Timer _timer;


        public void ConnectUser(User user)
        {
            if (ConnectedUser is null)
                ConnectedUser = user;
            else
            {
                throw new Exception("Another user is already connected.");
            }
        }

        public void DisconnectUser()
        {
            ConnectedUser = null;
        }

        public void StartCharging(DataForLogRandomization dataLogRnd, Guid gatewayId, string gatewayCode)
        {
            _timer = new System.Timers.Timer(dataLogRnd.LogIntervalSendingTime.TotalMilliseconds);

            _timer.Elapsed += (srcobj, e) => RandomizeAndSendLogs(dataLogRnd, gatewayId, gatewayCode);
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }


        private void RandomizeLogToSend(DataForLogRandomization dataLogRnd, Guid gatewayId, string gatewayCode)
        {
            if (ConnectedUser is not null)
            {
                decimal chargingSpeed = RandomizeSpeedLog(dataLogRnd.EVChargerLevelOfColumns, ConnectedUser);

                TimeSpan chargingTime = CalculateChargingTime(dataLogRnd);

                // The chargingSpeed is kW so I divide the chargingTime (seconds) for 3600 seconds to find the
                // consumption which is in kWh.
                decimal consumptionSoFar = CalculateConsumptionSoFar(chargingSpeed, dataLogRnd);

                decimal costSoFar = CalculateCostSoFar(consumptionSoFar);

                ChargingStationStatus status = DetermineStatusForTheLog();

                LogToSend = new()
                {
                    ChargingSpeed = chargingSpeed,
                    CostSoFar = costSoFar,
                    ChargingTime = chargingTime,
                    ColumnId = Id,
                    ConsumptionSoFar = consumptionSoFar,
                    Status = status,
                    ColumnNumber = Number,
                    GatewayCode = gatewayCode,
                    GatewayId = gatewayId,
                    UserId = ConnectedUser.Id
                };
            }
            else
            {
                // TODO --> Think if there's something to randomize for logs in the case there's no user connected to the column.
            }

        }

        private void SendLog()
        {
            if (LogToSend is not null)
            {
                // TODO --> Send log into a queue for the Display console to receive it.
                AnsiConsole.Write(
                new Panel(new Rows(
                    new Markup($"[bold yellow]Column ID:[/] {LogToSend.ColumnId}"),
                    new Markup($"[bold yellow]Column Number:[/] {LogToSend.ColumnNumber}"),
                    new Markup($"[bold yellow]Gateway ID:[/] {LogToSend.GatewayId}"),
                    new Markup($"[bold yellow]Gateway Code:[/] {LogToSend.GatewayCode}"),
                    new Markup($"[bold yellow]Connected User ID:[/] {LogToSend.UserId}"),
                    new Markup($"[bold yellow]Column Status:[/] {LogToSend.Status}"),
                    new Markup($"[bold yellow]Charging Time:[/] {LogToSend.ChargingTime.TotalSeconds} seconds"),
                    new Markup($"[bold yellow]Charging Speed:[/] {LogToSend.ChargingSpeed:F2} kW"),
                    new Markup($"[bold yellow]Consumption So Far:[/] {LogToSend.ConsumptionSoFar:F2} kWh"),
                    new Markup($"[bold yellow]Cost So Far:[/] {LogToSend.CostSoFar:F3} euro")
                ))
                .Header("Charging Log")
                .BorderColor(Spectre.Console.Color.Green)
                .DoubleBorder()
            );

                LastLogOfCurrentCharge = new()
                {
                    ChargingSpeed = LogToSend.ChargingSpeed,
                    ChargingTime = LogToSend.ChargingTime,
                    CostSoFar = LogToSend.CostSoFar,
                    ColumnId = Id,
                    ConsumptionSoFar = LogToSend.ConsumptionSoFar,
                    Status = LogToSend.Status,
                    ColumnNumber = LogToSend.ColumnNumber,
                    GatewayCode = LogToSend.GatewayCode,
                    GatewayId = LogToSend.GatewayId,
                    UserId = LogToSend.UserId
                };
                LogToSend = null;
            }
            else
            {
                throw new Exception("LogToSend is null");
            }
        }

        private void RandomizeAndSendLogs(DataForLogRandomization dataLogRnd, Guid gatewayId, string gatewayCode)
        {
            RandomizeLogToSend(dataLogRnd, gatewayId, gatewayCode);
            SendLog();
        }

        private decimal RandomizeSpeedLog(EVChargerLevel EVClevel, User user)
        {
            if(LastLogOfCurrentCharge is not null)
            {
                return LastLogOfCurrentCharge.ChargingSpeed;
            }

            Random rnd = new Random();
            decimal chargingSpeed = EVClevel.MinSpeed + (decimal)rnd.NextDouble() * (EVClevel.MaxSpeed - EVClevel.MinSpeed);

            // TODO --> User subscription

            return chargingSpeed;

        }

        private TimeSpan CalculateChargingTime(DataForLogRandomization dataLogRnd)
        {
            // TODO --> Probably I could calculate this better using the same timer I use to send the logs every 2 seconds.
            TimeSpan chargingTime = dataLogRnd.LogIntervalSendingTime;
            if (LastLogOfCurrentCharge is not null)
            {
                chargingTime = CalculateChargingTimeFromLastLog(dataLogRnd.LogIntervalSendingTime);
            }
            return chargingTime;

        }

        private TimeSpan CalculateChargingTimeFromLastLog(TimeSpan logIntervalSendingTime)
        {
            if (LastLogOfCurrentCharge is not null)
                return LastLogOfCurrentCharge.ChargingTime + logIntervalSendingTime;
            else throw new Exception("LastLogOfCurrentCharge is null");
        }

        private decimal CalculateConsumptionSoFar(decimal chargingSpeed, DataForLogRandomization dataLogRnd)
        {
            decimal consumptionSoFar = chargingSpeed * (decimal)dataLogRnd.LogIntervalSendingTime.TotalHours;
            if (LastLogOfCurrentCharge is not null)
            {
                consumptionSoFar += LastLogOfCurrentCharge.ConsumptionSoFar;
            }
            return consumptionSoFar;
        }


        private decimal CalculateCostSoFar(decimal consumptionSoFar)
        {
            // TODO --> Define how subscriptions work and modify the cost calculations.

            return consumptionSoFar * 0.11m; // TODO --> Parameterize the 0.11 which indicates €/kWh. 
        }

        private ChargingStationStatus DetermineStatusForTheLog()
        {
            return ChargingStationStatus.Charging; // TODO.
        }

    }
    public record LogDataDependingOnPreviowsLog
    {
        public decimal ConsuptionSoFar { get; init; }
        public TimeSpan LastName { get; init; }
    }

}
