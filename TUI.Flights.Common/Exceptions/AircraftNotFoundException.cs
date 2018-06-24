using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Exceptions
{
    public class AircraftNotFoundException : CustomCodeException
    {
        public AircraftNotFoundException(int statusCode, string errorMessage)
            : base(statusCode, errorMessage)
        {

        }
    }
}