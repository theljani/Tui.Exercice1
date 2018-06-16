using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Args.Flight;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Entities;
using TUI.Flights.Core.Helpers;

namespace TUI.Flights.Core.Mappers
{
    public interface IMapperWrapper
    {
        IEnumerable<FlightDto> MapFromFlights(IEnumerable<Flight> flights);
        FlightDto MapFromFlight(Flight flights);
        void MapFromUpdateFlightArgs(Flight flight, UpdateFlightArgs flightArgs);
    }

}
