using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUI.Flights.Common.ValidationAttributes;

namespace TUI.Flights.Common.Dtos.Flight
{
    public class FlightDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Flight number is mandatory")]
        public string FlightNumber { get; set; }

        public string AirportDeparture { get; set; }

        [Required(ErrorMessage = "Departure airport is mandatory")]
        [Range(1, int.MaxValue, ErrorMessage = "Departure Airport Id is not Valid.")]
        public int DepartureAirportId { get; set; }

        public string AirportDestination { get; set; }

        [Required(ErrorMessage = "Destination airport is mandatory")]
        [Range(1, int.MaxValue, ErrorMessage = "Destination Airport Id is not Valid.")]
        public int DestinationAirportId { get; set; }

        [Required(ErrorMessage = "Flight date is mandatory ")]
        public DateTime FlightDate { get; set; }

        [Required(ErrorMessage = "Flight time is mandatory ")]
        public DateTime FlightTime { get; set; }

        public string Aircraft { get; set; }

        [Required(ErrorMessage = "Aircraft model is mandatory ")]
        [Range(1, int.MaxValue, ErrorMessage = "Aircraft id is not Valid.")]
        public int AircraftId { get; set; }

        // Calculated values
        public double Distance { get; set; }
        public DateTime EstimatedArrivalTime { get; set; }
        public double EstimatedFuelNeeded { get; set; }
        public double EstimatedFlightDuration { get; set; }
    }
}