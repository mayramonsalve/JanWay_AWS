using System;
using System.Collections.Generic;

namespace JWA.Core.Entities
{
    public partial class SystemStatus : BaseEntity
    {
        public DateTime Date { get; set; }
        public double SelenoidTemperature { get; set; }
        public int BatteryLevel { get; set; }
        public int Health { get; set; }
        public int Performance { get; set; }
        public int UnitId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
