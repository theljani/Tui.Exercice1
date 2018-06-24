using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Exceptions
{
    public class SameDepartureAndDestinationAirportsException : CustomCodeException
    {
        public SameDepartureAndDestinationAirportsException(int statusCode, string errorMessage)
            : base(statusCode, errorMessage)
        {

        }
    }
}
