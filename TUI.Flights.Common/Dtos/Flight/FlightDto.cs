using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Dtos.Flight
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }

        public string AirportDeparture { get; set; }
        public int AirportDepartureId { get; set; }

        public string AirportDestination { get; set; }
        public int AirportDestinationId { get; set; }

        public DateTime FlightDate { get; set; }
        public DateTime FlightTime { get; set; }

        public string Aircraft { get; set; }
        public int AircraftId { get; set; }

        // Calculated values
        public double Distance { get; set; }
        public DateTime EstimatedArrivalTime { get; set; }
        public double EstimatedFuelNeeded { get; set; }
        public double EstimatedFlightDuration { get; set; }
    }
}
