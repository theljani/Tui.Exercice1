using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Args.Common;

namespace TUI.Flights.Common.Args.Aircraft
{
    public class SearchAircraftsArgs
    {
        public PaginationArgs Pagination { get; set; }
        public AircraftSearchFiltersArgs Filters { get; set; }

        public SearchAircraftsArgs()
        {
            Pagination = new PaginationArgs();
        }
    }
}
