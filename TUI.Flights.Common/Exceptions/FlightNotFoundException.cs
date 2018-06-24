using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Exceptions
{
    public class FlightNotFoundException : CustomCodeException
    {
        public FlightNotFoundException(int statusCode, string errorMessage)
            : base(statusCode, errorMessage)
        {

        }
    }
}
