using EdgeComputerSimulator.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeComputerSimulator.Library.Models
{
    public class EVChargerLevel
    {

        public decimal MinSpeed { get; private set; } // Unit of measure: kW
        public decimal MaxSpeed { get; private set; } // Unit of measure: kW
        public EVCLevel EVClevel { get; init; }

        public EVChargerLevel(EVCLevel eVClevel)
        {
            EVClevel = eVClevel;
            SetSpeedRange();
        }

        private void SetSpeedRange()
        {
            switch (EVClevel)
            {
                case EVCLevel.Level1:
                    MinSpeed = 1.0m;
                    MaxSpeed = 1.8m;
                    break;

                case EVCLevel.Level2:
                    MinSpeed = 3.0m;
                    MaxSpeed = 22.0m;
                    break;

                case EVCLevel.Level3:
                    MinSpeed = 30.0m;
                    MaxSpeed = 360.0m;
                    break;
            }
        }


    }
}
