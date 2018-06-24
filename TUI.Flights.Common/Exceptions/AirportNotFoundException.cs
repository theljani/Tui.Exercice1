using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Exceptions
{
    public class AirportNotFoundException : CustomCodeException
    {
        public AirportNotFoundException(int statusCode, string errorMessage)
            : base(statusCode, errorMessage)
        {

        }
    }
}

