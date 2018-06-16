using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TUI.Flights.Common.Args.Flight
{
    public class CreateFlightArgs
    {
        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public int DepartureAirport { get; set; }

        [Required]
        public int DestinationAirport { get; set; }

        [Required]
        public DateTime FlightDate { get; set; }

        [Required]
        public DateTime FlightTime { get; set; }

        [Required]
        public int Aircraft { get; set; }
    }
}
