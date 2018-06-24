using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Core.Helpers
{
    public static class ErrorMessages
    {
        public const string FLIGHT_NOT_FOUND = "Flight with id {0} is not found in the system.";
        public const string DEPARTURE_AIRPORT_NOT_FOUND = "Departure airport with id {0} is not found in the system.";
        public const string DESTINATION_AIRPORT_NOT_FOUND = "Destination airport with id {0} is not found in the system.";
        public const string AIRCRAFT_NOT_FOUND = "Aircraft with id {0} is not found in the system.";
        public const string SAME_DEPARTURE_AND_DESTINATION = "You cannot select the same airport as departure and destination in the same time.";
        public const string DUPLICATED_FLIGHT_NUMBER = "Another flight withe the same number already exists in the system.";
    }
}
