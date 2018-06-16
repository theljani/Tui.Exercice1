using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.ValueObjects
{
    public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string TimeZone { get; set; }

        public Location() { }

        public Location(string country, string city, string state, string timeZone)
        {
            Country = country;
            City = city;
            State = state;
            TimeZone = timeZone;
        }
    }
}