using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TUI.Flights.Common.Entities.Base;

namespace TUI.Flights.Common.Entities
{
    public class Flight : Entity
    {
        public string FlightNumber { get; set; }

        [ForeignKey(nameof(AirportDepartureId))]
        public virtual Airport Departure { get; set; }
        public int AirportDepartureId { get; set; }

        [ForeignKey(nameof(AirportDestinationId))]
        public virtual Airport Destination { get; set; }
        public int AirportDestinationId { get; set; }

        public DateTime FlightDate { get; set; }
        public DateTime FlightTime { get; set; }

        // TODO : Aircraft.
        [ForeignKey(nameof(AircraftId))]
        public virtual Aircraft Aircraft { get; set; }
        public int AircraftId { get; set; }

        // Calculated values
        public double Distance { get; set; }
        public DateTime EstimatedArrivalTime { get; set; }
        public double EstimatedFuelNeeded { get; set; }
        public double EstimatedFlightDuration { get; set; }
    }
}
