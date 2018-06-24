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
using TUI.Flights.Common.Enums;
using TUI.Flights.Common.Exceptions;
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
        private readonly IMapperWrapper _mapper;
        private readonly FlightHelper _flightHelper;

        public FlightServices(IRepository<Flight> flightsRepository,
            IRepository<Airport> airportRepository,
            IRepository<Aircraft> aircrafttRepository,
            IMapperWrapper mapper)
        {
            _flightsRepository = flightsRepository ?? throw new ArgumentNullException("flightsRepository");
            _airportRepository = airportRepository ?? throw new ArgumentNullException("airportRepository");
            _aircrafttRepository = aircrafttRepository ?? throw new ArgumentNullException("aircrafttRepository");

            _mapper = mapper ?? throw new ArgumentNullException("autoMapper");
            _flightHelper = new FlightHelper();
        }

        #region IFlightServices Methods 

        public async Task<FlightDto> GetFlight(GetFlightArgs args)
        {

            var flight = await _flightsRepository.GetAsync(args.FlightId);

            if (flight == null)
                throw new FlightNotFoundException(Convert.ToInt32(StatusCodes.NotFound), string.Format(ErrorMessages.FLIGHT_NOT_FOUND, args.FlightId));

            // We can use this code if we want to use Explicit loading instead of Lazy loading
            //_flightsRepository.GetEntry(flight).Reference("Departure").Load();
            //_flightsRepository.GetEntry(flight).Reference("Destination").Load();
            //_flightsRepository.GetEntry(flight).Reference("Aircraft").Load();

            return _mapper.MapFromFlight(flight);
        }

        public async Task<FlightListDto> GetAllFlights(PaginationArgs pagination)
        {
            var flights = await _flightsRepository.GetAllAsync();

            var flightsDto = _mapper.MapFromFlights(flights.ToList()
                .OrderByDescending(e => e.CreationDate)
                .Skip(pagination.StartIndex.Value)
                .Take(pagination.PageSize.Value));

            int total = _flightsRepository.GetTotal();

            return new FlightListDto
            {
                Items = flightsDto,
                Total = total
            };
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

            var flights = await _flightsRepository.SearchAsync(expression);

            return _mapper.MapFromFlights(flights.Skip(searchArgs.Pagination.StartIndex.Value).Take(searchArgs.Pagination.PageSize.Value));
        }

        public async Task<FlightDto> CreateFlight(CreateFlightArgs args)
        {
            CheckFlightUnicity(null, args.FlightNumber);

            var flightRelatedData = GetFlightRelatedData(args.DepartureAirportId, args.DestinationAirportId, args.AircraftId);

            var departure = flightRelatedData.Departure;
            var destination = flightRelatedData.Destination;
            var aircraft = flightRelatedData.Aircraft;

            var distance = _flightHelper.CalculateDistance(new GpsCoordinates(departure.Latitude, departure.Longitude), new GpsCoordinates(departure.Latitude, destination.Longitude));
            var estimatedDuration = _flightHelper.CalculateDuration(distance, aircraft.MilesPerHour);

            var newFlight = new Flight
            {
                FlightNumber = args.FlightNumber,
                FlightDate = args.FlightDate,
                FlightTime = args.FlightTime,
                AirportDepartureId = args.DepartureAirportId,
                AirportDestinationId = args.DestinationAirportId,
                AircraftId = args.AircraftId,
                Distance = distance,
                EstimatedFlightDuration = estimatedDuration,
                EstimatedFuelNeeded = _flightHelper.CalculateFuelConsumption(estimatedDuration, aircraft.ConsumptionKgPerH, aircraft.TakeOffEffort),
                CreationDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            var createdFlight = await _flightsRepository.AddAsync(newFlight);

            return _mapper.MapFromFlight(createdFlight);
        }

        public async Task<FlightDto> UpdateFlight(UpdateFlightArgs args)
        {
            var flightToUpdate = await _flightsRepository.GetAsync(args.Id);

            if (flightToUpdate == null)
            {
                throw new FlightNotFoundException(Convert.ToInt32(StatusCodes.NotFound), string.Format(ErrorMessages.FLIGHT_NOT_FOUND, args.Id));
            }

            CheckFlightUnicity(args.Id, args.FlightNumber);

            var flightRelatedData = GetFlightRelatedData(args.DepartureAirportId, args.DestinationAirportId, args.AircraftId);

            var departure = flightRelatedData.Departure;
            var destination = flightRelatedData.Destination;
            var aircraft = flightRelatedData.Aircraft;

            var distance = _flightHelper.CalculateDistance(new GpsCoordinates(departure.Latitude, departure.Longitude), new GpsCoordinates(departure.Latitude, destination.Longitude));
            var estimatedDuration = _flightHelper.CalculateDuration(distance, aircraft.MilesPerHour);


            _mapper.MapFromUpdateFlightArgs(flightToUpdate, args);

            flightToUpdate.Distance = distance;
            flightToUpdate.EstimatedFlightDuration = estimatedDuration;
            flightToUpdate.EstimatedFuelNeeded = _flightHelper.CalculateFuelConsumption(estimatedDuration, aircraft.ConsumptionKgPerH, aircraft.TakeOffEffort);

            var updatedFlight = await _flightsRepository.UpdateAsync(flightToUpdate);
            return _mapper.MapFromFlight(updatedFlight);
        }

        public async Task<bool> DeleteFlight(int flightId)
        {
            var flightToDelete = _flightsRepository.Get(flightId);

            if (flightToDelete == null)
                throw new FlightNotFoundException(Convert.ToInt32(StatusCodes.NotFound), string.Format(ErrorMessages.FLIGHT_NOT_FOUND, flightId));

            var deletedFlight = await _flightsRepository.DeleteAsync(flightId);

            return true;
        }

        public FlightRelatedData GetFlightRelatedData(int departureAirportId, int destinationAirportId, int aircraftId)
        {
            if (departureAirportId == destinationAirportId)
                throw new SameDepartureAndDestinationAirportsException(Convert.ToInt32(StatusCodes.BadRequest), ErrorMessages.SAME_DEPARTURE_AND_DESTINATION);

            var departure = _airportRepository.Get(departureAirportId);

            if (departure == null)
            {
                throw new AirportNotFoundException(Convert.ToInt32(StatusCodes.NotFound), string.Format(ErrorMessages.DEPARTURE_AIRPORT_NOT_FOUND, departureAirportId));
            }

            var destination = _airportRepository.Get(destinationAirportId);

            if (destination == null)
            {
                throw new AirportNotFoundException(Convert.ToInt32(StatusCodes.NotFound), string.Format(ErrorMessages.DESTINATION_AIRPORT_NOT_FOUND, destinationAirportId));
            }

            var aircraft = _aircrafttRepository.Get(aircraftId);

            if (aircraft == null)
            {
                throw new AircraftNotFoundException(Convert.ToInt32(StatusCodes.NotFound), string.Format(ErrorMessages.AIRCRAFT_NOT_FOUND, aircraftId));
            }

            return new FlightRelatedData
            {
                Departure = departure,
                Destination = destination,
                Aircraft = aircraft
            };
        }

        public void CheckFlightUnicity(int? flightId, string flightNumber)
        {
            Expression<Func<Flight, bool>> expression = (Flight f)
                => flightId != null ? (f.Id != flightId && f.FlightNumber.ToLower() == flightNumber.ToLower())
                : (f.FlightNumber.ToLower() == flightNumber.ToLower());


            var exists = _flightsRepository.Search(expression);

            if (exists.Any())
                throw new DuplicateFlightException(Convert.ToInt32(StatusCodes.BadRequest), ErrorMessages.DUPLICATED_FLIGHT_NUMBER);
        }
        #endregion
    }
}
