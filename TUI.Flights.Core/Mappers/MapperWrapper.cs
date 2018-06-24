using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Entities;
using System.Linq;
using TUI.Flights.Core.Helpers;
using TUI.Flights.Common.Args.Flight;

namespace TUI.Flights.Core.Mappers
{
    public class MapperWrapper : IMapperWrapper
    {
        public IEnumerable<FlightDto> MapFromFlights(IEnumerable<Flight> flights)
        {
            return flights.Select(f => MapFromFlight(f));
        }

        public FlightDto MapFromFlight(Flight flight)
        {
            if (flight != null)
            {
                return new FlightDto
                {
                    Id = flight.Id,
                    FlightNumber = flight.FlightNumber,
                    Distance = flight.Distance,
                    Aircraft = flight.Aircraft != null ? flight.Aircraft.Code : string.Empty,
                    AircraftId = flight.AircraftId,
                    AirportDeparture = flight.Departure != null ? flight.Departure.Name : string.Empty,
                    AirportDestination = flight.Destination != null ? flight.Destination.Name : string.Empty,
                    DepartureAirportId = flight.AirportDepartureId,
                    DestinationAirportId = flight.AirportDestinationId,
                    EstimatedArrivalTime = flight.EstimatedArrivalTime,
                    EstimatedFlightDuration = flight.EstimatedFlightDuration,
                    EstimatedFuelNeeded = flight.EstimatedFuelNeeded,
                    FlightDate = flight.FlightDate,
                    FlightTime = flight.FlightTime
                };
            }

            return null;
        }

        public void MapFromUpdateFlightArgs(Flight flight, UpdateFlightArgs flightArgs)
        {
            flight.Id = flightArgs.Id;
            flight.FlightNumber = flightArgs.FlightNumber;
            flight.AirportDepartureId = flightArgs.DepartureAirportId;
            flight.AirportDestinationId = flightArgs.DestinationAirportId;
            flight.FlightDate = flightArgs.FlightDate;
            flight.FlightTime = flightArgs.FlightTime;
            flight.AircraftId = flightArgs.AircraftId;
        }
    }
}
