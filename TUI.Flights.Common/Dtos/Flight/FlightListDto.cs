using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Dtos.Flight
{
    public class FlightListDto
    {
        public IEnumerable<FlightDto> Items { get; set; }
        public int Total { get; set; }
    }
}
