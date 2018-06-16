using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Args.Flight;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Entities;
using TUI.Flights.Core.Helpers;
using TUI.Flights.Core.Mappers;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Core.Services.FlightServices
{
    public class FlightServices : IFlightServices
    {
        private readonly IRepository<Flight> _flightsRepository;
        private readonly IRepository<Airport> _airportRepository;
        private readonly IRepository<Aircraft> _aircrafttRepository;
        private readonly IMapper _autoMapper;
        private readonly IMapperWrapper _mapper;

        public FlightServices(IRepository<Flight> flightsRepository,
            IRepository<Airport> airportRepository,
            IRepository<Aircraft> aircrafttRepository,
            IMapperWrapper mapper)
        {
            _flightsRepository = flightsRepository ?? throw new ArgumentNullException("flightsRepository");
            _airportRepository = airportRepository ?? throw new ArgumentNullException("airportRepository");
            _aircrafttRepository = aircrafttRepository ?? throw new ArgumentNullException("aircrafttRepository");

            _mapper = mapper ?? throw new ArgumentNullException("autoMapper");
        }

        #region IFlightServices Methods 
        public async Task<FlightDto> GetFlight(GetFlightArgs args)
        {

            var flight = await _flightsRepository.GetAsync(args.FlightId);
            // We can use this code if we want to use Explicit loading instead of Lazy loading

            //_flightsRepository.GetEntry(flight).Reference("Departure").Load();
            //_flightsRepository.GetEntry(flight).Reference("Destination").Load();
            //_flightsRepository.GetEntry(flight).Reference("Aircraft").Load();

            return _mapper.MapFromFlight(flight);
        }

        public async Task<IEnumerable<FlightDto>> GetAllFlights(PaginationArgs pagination)
        {
            var flights = await _flightsRepository.GetAllAsync(pagination.PageSize.Value, pagination.StartIndex.Value);

            return _mapper.MapFromFlights(flights);
            //return _autoMapper.Map<IEnumerable<FlightDto>>(flights);
        }

        public async Task<IEnumerable<FlightDto>> SearchFlights(SearchFlightsArgs searchArgs)
        {
            Expression<Func<Flight, bool>> expression = null;

            if (searchArgs != null && searchArgs.Filters != null)
            {
                expression = (Flight f) =>
                                        (f.AirportDepartureId == searchArgs.Filters.DepartureAirport
                                            || f.AirportDestinationId == searchArgs.Filters.DestinationAirport
                                            || f.FlightDate == searchArgs.Filters.FlightDate);
            }

            var flights = await _flightsRepository.SearchAsync(expression, searchArgs.Pagination.PageSize.Value, searchArgs.Pagination.StartIndex.Value);

            return _mapper.MapFromFlights(flights);
            //return _autoMapper.Map<IEnumerable<FlightDto>>(flights);
        }

        public async Task<FlightDto> CreateFlight(CreateFlightArgs args)
        {
            var departure = _airportRepository.Get(args.DepartureAirport);
            var destination = _airportRepository.Get(args.DestinationAirport);
            var aircraft = _aircrafttRepository.Get(args.Aircraft);

            var distance = FlightHelper.CalculateDistance(new GpsCoordinates(departure.Latitude, departure.Longitude), new GpsCoordinates(departure.Latitude, destination.Longitude));
            var estimatedDuration = FlightHelper.CalculateDuration(distance, aircraft.MilesPerHour);

            var newFlight = new Flight
            {
                FlightNumber = args.FlightNumber,
                FlightDate = args.FlightDate,
                FlightTime = args.FlightTime,
                AirportDepartureId = args.DepartureAirport,
                AirportDestinationId = args.DestinationAirport,
                AircraftId = args.Aircraft,
                Distance = distance,
                EstimatedFlightDuration = estimatedDuration,
                EstimatedFuelNeeded = FlightHelper.CalculateFuelConsumption(estimatedDuration, aircraft.ConsumptionKgPerH, aircraft.TakeOffEffort),
                CreationDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            var createdFlight = await _flightsRepository.AddAsync(newFlight);

            return _mapper.MapFromFlight(createdFlight);
        }

        public async Task<FlightDto> UpdateFlight(UpdateFlightArgs args)
        {
            var departure = _airportRepository.Get(args.AirportDepartureId);
            var destination = _airportRepository.Get(args.AirportDestinationId);
            var aircraft = _aircrafttRepository.Get(args.AircraftId);

            var distance = FlightHelper.CalculateDistance(new GpsCoordinates(departure.Latitude, departure.Longitude), new GpsCoordinates(departure.Latitude, destination.Longitude));
            var estimatedDuration = FlightHelper.CalculateDuration(distance, aircraft.MilesPerHour);

            var flightToUpdate = await _flightsRepository.GetAsync(args.Id);

            _mapper.MapFromUpdateFlightArgs(flightToUpdate, args);
            flightToUpdate.Distance = distance;
            flightToUpdate.EstimatedFlightDuration = estimatedDuration;
            flightToUpdate.EstimatedFuelNeeded = FlightHelper.CalculateFuelConsumption(estimatedDuration, aircraft.ConsumptionKgPerH, aircraft.TakeOffEffort);

            var updatedFlight = await _flightsRepository.UpdateAsync(flightToUpdate);
            return _mapper.MapFromFlight(updatedFlight);
        }

        public async Task<bool> DeleteFlight(int flightId)
        {
            var deletedFlight = await _flightsRepository.DeleteAsync(flightId);

            return true;
        }
        #endregion
    }
}
