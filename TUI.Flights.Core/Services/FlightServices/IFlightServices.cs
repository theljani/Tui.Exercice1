using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Args.Flight;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Entities;

namespace TUI.Flights.Core.Services.FlightServices
{
    public interface IFlightServices
    {
        Task<IEnumerable<FlightDto>> SearchFlights(SearchFlightsArgs searchArgs);
        Task<FlightListDto> GetAllFlights(PaginationArgs pagination);
        Task<FlightDto> GetFlight(GetFlightArgs args);
        Task<FlightDto> CreateFlight(CreateFlightArgs args);
        Task<FlightDto> UpdateFlight(UpdateFlightArgs args);
        Task<bool> DeleteFlight(int flighId);
        void CheckFlightUnicity(int? flightId, string flightNumber);
        FlightRelatedData GetFlightRelatedData(int departureAirportId, int destinationAirportId, int aircraftId);
    }
}
