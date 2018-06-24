using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Exceptions
{
    public class DuplicateFlightException : CustomCodeException
    {
        public DuplicateFlightException(int statusCode, string errorMessage)
            : base(statusCode, errorMessage)
        {

        }
    }
}