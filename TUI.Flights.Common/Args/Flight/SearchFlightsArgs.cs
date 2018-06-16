using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Args.Common;

namespace TUI.Flights.Common.Args.Flight
{
    public class SearchFlightsArgs
    {
        public PaginationArgs Pagination { get; set; }
        public FlightSearchFiltersArgs Filters { get; set; }

        public SearchFlightsArgs()
        {
            Pagination = new PaginationArgs();
        }
    }
}
