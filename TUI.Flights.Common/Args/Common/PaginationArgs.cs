using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Args.Common
{
    public class PaginationArgs
    {
        public int? PageSize { get; set; }
        public int? StartIndex { get; set; }

        public PaginationArgs()
        {
            PageSize = 10;
            StartIndex = 0;
        }
    }
}
