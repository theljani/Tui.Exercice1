using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUI.Flights.Core.Helpers
{
    public class GpsCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GpsCoordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
