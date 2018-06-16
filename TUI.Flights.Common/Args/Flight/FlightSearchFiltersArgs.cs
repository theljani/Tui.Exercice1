using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Args.Flight
{
    public class FlightSearchFiltersArgs
    {
        public int DepartureAirport { get; set; }
        public int DestinationAirport { get; set; }
        public DateTime FlightDate { get; set; }
    }
}
