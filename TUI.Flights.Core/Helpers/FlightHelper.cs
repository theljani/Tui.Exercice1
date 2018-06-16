using BAMCIS.GIS;
using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Core.Helpers
{
    public static class FlightHelper
    {
        public static double CalculateDistance(GpsCoordinates location1, GpsCoordinates location2)
        {
            GeoCoordinate departure = new GeoCoordinate(location1.Latitude, location1.Longitude);
            GeoCoordinate Destination = new GeoCoordinate(location2.Latitude, location2.Longitude);

            return Math.Round(departure.DistanceTo(Destination, DistanceType.MILES), 2);
        }

        public static double CalculateDuration(double totalDistance, double distancePerMile)
        {
            return Math.Round(totalDistance / distancePerMile, 2);
        }

        public static double CalculateFuelConsumption(double totalDuration, double consumptionPerHour, double takeOffEffort)
        {
            return Math.Round(totalDuration * consumptionPerHour + takeOffEffort);
        }
    }
}
