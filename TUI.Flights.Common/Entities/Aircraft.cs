using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Entities.Base;

namespace TUI.Flights.Common.Entities
{
    public class Aircraft : Entity
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public double ConsumptionKgPerH { get; set; }
        public double TakeOffEffort { get; set; }
        public double MilesPerHour { get; set; }
        public DateTime ConstructionYear { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
    }
}
