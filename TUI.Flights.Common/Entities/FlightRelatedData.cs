using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Entities
{
    public class FlightRelatedData
    {
        public Airport Departure { get; set; }
        public Airport Destination { get; set; }
        public Aircraft Aircraft { get; set; }
    }
}
