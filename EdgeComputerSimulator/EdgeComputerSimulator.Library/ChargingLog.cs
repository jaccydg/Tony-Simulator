using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library
{
    public class ChargingLog
    {
        Guid Id { get; } = Guid.NewGuid();
        public decimal Speed { get; private set; }
        public decimal ConsuptionSoFar { get; private set; }
        public decimal CostSoFar { get; private set; }
        public TimeSpan ChargingTime { get; private set; }
        public Guid UserId { get; private set; }
        public Column Column { get; set; }


        public ChargingLog(decimal speed, decimal consuptionSoFar, decimal costSoFar, TimeSpan chargingTime, Guid userId, Column column)
        {
            Speed = speed;
            ConsuptionSoFar = consuptionSoFar;
            CostSoFar = costSoFar;
            ChargingTime = chargingTime;
            UserId = userId;
            Column = column;
        }

    }
}
