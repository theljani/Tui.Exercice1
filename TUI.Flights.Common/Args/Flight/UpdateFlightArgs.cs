using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUI.Flights.Common.ValidationAttributes;

namespace TUI.Flights.Common.Args.Flight
{
    public class UpdateFlightArgs
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public int DepartureAirportId { get; set; }

        [Required]
        public int DestinationAirportId { get; set; }

        [Required]
        public DateTime FlightDate { get; set; }

        [Required]
        public DateTime FlightTime { get; set; }

        [Required]
        public int AircraftId { get; set; }
    }
}
