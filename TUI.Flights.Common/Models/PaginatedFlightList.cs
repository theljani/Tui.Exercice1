using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Dtos.Flight;

namespace TUI.Flights.Common.Models
{
    public class PaginatedFlightList
    {
        public IEnumerable<FlightDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        = 5;
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }
}
