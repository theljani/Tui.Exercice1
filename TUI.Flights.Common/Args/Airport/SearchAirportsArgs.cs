using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Args.Common;

namespace TUI.Flights.Common.Args.Airport
{
    public class SearchAirportsArgs
    {
        public PaginationArgs Pagination { get; set; }
        public AirportSearchFiltersArgs Filters { get; set; }

        public SearchAirportsArgs()
        {
            Pagination = new PaginationArgs();
        }
    }
}
