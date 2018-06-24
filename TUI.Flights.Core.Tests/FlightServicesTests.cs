using BAMCIS.GIS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Args.Flight;
using TUI.Flights.Common.Entities;
using TUI.Flights.Common.Exceptions;
using TUI.Flights.Core.Helpers;
using TUI.Flights.Core.Mappers;
using TUI.Flights.Core.Services.FlightServices;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Core.Tests
{
    [TestClass]
    public class FlightServicesTests
    {

        private Mock<IRepository<Flight>> _flightRepositoryMock;
        private Mock<IRepository<Airport>> _airportRepositoryMock;
        private Mock<IRepository<Aircraft>> _aircraftRepositoryMock;
        private IMapperWrapper _mapperWrapper;

        private List<Flight> _flightsRecordsMock;
        private List<Airport> _airportsRecordsMock;
        private List<Aircraft> _aircraftsRecordsMock;

        [TestInitialize]
        public void Init()
        {
            var random = new Random();
            _mapperWrapper = new MapperWrapper();
            _flightsRecordsMock = new List<Flight>();

            _flightRepositoryMock = new Mock<IRepository<Flight>>();
            _airportRepositoryMock = new Mock<IRepository<Airport>>();
            _aircraftRepositoryMock = new Mock<IRepository<Aircraft>>();

            for (int i = 1; i < 10; i++)
            {
                _flightsRecordsMock.Add(new Flight
                {
                    Id = i,
                    FlightNumber = "FakeFlight_" + i,
                    AirportDepartureId = i,
                    AirportDestinationId = i + 1,
                    AircraftId = i,
                    FlightDate = DateTime.Now,
                    FlightTime = DateTime.Now
                });
            }

            _airportsRecordsMock = new List<Airport>();
            for (int i = 1; i < 10; i++)
            {
                _airportsRecordsMock.Add(new Airport
                {
                    Id = i,
                    Name = "FakeAirport_" + i,
                    Latitude = random.Next(-90, 90),
                    Longitude = random.Next(-90, 90)
                });
            }

            _aircraftsRecordsMock = new List<Aircraft>();
            for (int i = 1; i < 10; i++)
            {
                _aircraftsRecordsMock.Add(new Aircraft
                {
                    Id = i,
                    Code = "FakeAircraft_" + i,
                    MilesPerHour = random.Next(100, 300),
                    TakeOffEffort = random.Next(30, 50),
                    ConsumptionKgPerH = random.Next(300, 800)
                });
            }

            #region Flight Repository Mock Setup
            _flightRepositoryMock.Setup(repo => repo.Get(It.IsAny<object>()))
                .Returns((int flightId) => _flightsRecordsMock.Where(f => f.Id.Equals(flightId)).FirstOrDefault());

            _flightRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<object>()))
                .Returns(async (int flightId) => await _flightsRecordsMock.ToAsyncEnumerable().Where(e => e.Id == flightId).FirstOrDefault());

            _flightRepositoryMock.Setup(repo => repo.GetAll())
                .Returns(_flightsRecordsMock.AsQueryable());

            _flightRepositoryMock.Setup(repo => repo.GetAllAsync())
                .Returns(async () => await _flightsRecordsMock.ToAsyncEnumerable().ToList());

            _flightRepositoryMock.Setup(repo => repo.GetTotal())
                .Returns(_flightsRecordsMock.Count());

            _flightRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Flight>()))
                .Returns(async (Flight flight) => flight);

            _flightRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Flight>()))
                .Returns(async (Flight flight) => flight);

            _flightRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(async (int flightId) => await _flightsRecordsMock.ToAsyncEnumerable().Where(f => f.Id == flightId).FirstOrDefault());
            #endregion

            #region Airport Repository Mock Setup
            _airportRepositoryMock.Setup(repo => repo.Get(It.IsAny<object>()))
                    .Returns((int airportId) => _airportsRecordsMock.Where(f => f.Id.Equals(airportId)).FirstOrDefault());

            _airportRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<object>()))
                .Returns(async (int airportId) => await _airportsRecordsMock.ToAsyncEnumerable().Where(e => e.Id == airportId).FirstOrDefault());
            #endregion

            #region Aircraft Repository Mock Setup
            _aircraftRepositoryMock.Setup(repo => repo.Get(It.IsAny<object>()))
                .Returns((int aircraftId) => _aircraftsRecordsMock.Where(f => f.Id.Equals(aircraftId)).FirstOrDefault());

            _aircraftRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<object>()))
                .Returns(async (int aircraftId) => await _aircraftsRecordsMock.ToAsyncEnumerable().Where(e => e.Id == aircraftId).FirstOrDefault());
            #endregion

        }

        #region Get Flight
        [TestMethod]
        public async Task FlightServicesTests_Get_ShouldReturnAFlightById()
        {
            //Arrange
            var flightId = _flightsRecordsMock.First().Id;
            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Expected 
            var expectFlight = _mapperWrapper.MapFromFlight(_flightsRecordsMock.First());

            // Act

            var currentFlight = await flightServices.GetFlight(new GetFlightArgs
            {
                FlightId = flightId
            });

            // Assert
            Check.That(currentFlight).IsNotNull();
            Check.That(currentFlight).HasFieldsWithSameValues(expectFlight);
            _flightRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public void FlightServicesTests_Get_ShouldThrowFlightNotFoundExceptionWhenFlightDoesNotExist()
        {
            //Arrange
            var flightId = 76767;
            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act && Assert
            Check.ThatCode(async () => await flightServices.GetFlight(new GetFlightArgs
            {
                FlightId = flightId
            }))
            .Throws<FlightNotFoundException>()
            .WithMessage(string.Format(ErrorMessages.FLIGHT_NOT_FOUND, flightId));
        }

        #endregion

        #region Get All Flights
        [TestMethod]
        public async Task FlightServicesTests_GetAllFlights_ShouldReturnAllFlights()
        {
            // Arrange
            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Expected
            var expectedFlights = _mapperWrapper.MapFromFlights(_flightsRecordsMock);

            // Act
            var currentFlights = await flightServices.GetAllFlights(new PaginationArgs
            {
                PageSize = int.MaxValue,
                StartIndex = 0
            });

            // Assert
            Check.That(currentFlights).IsNotNull();
            Check.That(currentFlights.Total).IsEqualTo(expectedFlights.Count());
            _flightRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        #endregion

        #region Create New Flight
        [TestMethod]
        public async Task FlightServicesTests_CreateNewFlight_ShouldAddSuccessfulyNewFlight()
        {
            // Arrange
            _flightRepositoryMock.Setup(repo => repo.Search(It.IsAny<Expression<Func<Flight, bool>>>()))
                .Returns(new List<Flight>().AsQueryable());

            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "FakeFlight_" + 50,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            var createdFlight = await flightServices.CreateFlight(newFlight);

            // Assert
            Check.That(createdFlight).IsNotNull();
            _flightRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Flight>()), Times.Once);
        }

        [TestMethod]
        public void FlightServicesTests_CreateNewFlight_ShouldThrowSameDepartureAndDestinationAirportsException()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "FakeFlight_30",
                DepartureAirportId = 1,
                DestinationAirportId = 1,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.CreateFlight(newFlight))
                .Throws<SameDepartureAndDestinationAirportsException>()
                .WithMessage(ErrorMessages.SAME_DEPARTURE_AND_DESTINATION);
        }

        [TestMethod]
        public void FlightServicesTests_CreateNewFlight_ShouldThrowDepartureAirportNotFoundException()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "FakeFlight_30",
                DepartureAirportId = 111111,
                DestinationAirportId = 1,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.CreateFlight(newFlight))
                .Throws<AirportNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.DEPARTURE_AIRPORT_NOT_FOUND, newFlight.DepartureAirportId));
        }

        [TestMethod]
        public void FlightServicesTests_CreateNewFlight_ShouldThrowDestinationAirportNotFoundException()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "FakeFlight_30",
                DepartureAirportId = 1,
                DestinationAirportId = 232323,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.CreateFlight(newFlight))
                .Throws<AirportNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.DESTINATION_AIRPORT_NOT_FOUND, newFlight.DestinationAirportId));
        }

        [TestMethod]
        public void FlightServicesTests_CreateNewFlight_ShouldThrowAircraftNotFoundException()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "FakeFlight_30",
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                AircraftId = 134343,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.CreateFlight(newFlight))
                .Throws<AircraftNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.AIRCRAFT_NOT_FOUND, newFlight.AircraftId));
        }

        [TestMethod]
        public void FlightServicesTests_CreateNewFlight_ShouldThrowDuplicateFlightException()
        {
            // Arrange
            _flightRepositoryMock.Setup(repo => repo.Search(It.IsAny<Expression<Func<Flight, bool>>>()))
                .Returns(new List<Flight>
                {
                    _flightsRecordsMock.First()
                }.AsQueryable());

            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "FakeFlight_1",
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.CreateFlight(newFlight))
                .Throws<DuplicateFlightException>()
                .WithMessage(ErrorMessages.DUPLICATED_FLIGHT_NUMBER);
        }

        #endregion

        #region Update a Flight
        [TestMethod]
        public async Task FlightServicesTests_UpdateFlight_ShouldUpdateSuccessfulyFlight()
        {
            // Arrange
            _flightRepositoryMock.Setup(repo => repo.Search(It.IsAny<Expression<Func<Flight, bool>>>()))
                .Returns(new List<Flight>().AsQueryable());

            var flightToUpdate = new UpdateFlightArgs
            {
                Id = 1,
                FlightNumber = "Updated_FakeFlight_1",
                DepartureAirportId = 3,
                DestinationAirportId = 4,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            var updatedFlight = await flightServices.UpdateFlight(flightToUpdate);

            // Assert
            Check.That(updatedFlight).IsNotNull();
            _flightRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Flight>()), Times.Once);
        }

        [TestMethod]
        public void FlightServicesTests_UpdateFlight_ShouldThrowSameDepartureAndDestinationAirportsException()
        {
            // Arrange
            var flightToUpdate = new UpdateFlightArgs
            {
                Id = 1,
                FlightNumber = "FakeFlight_1",
                DepartureAirportId = 1,
                DestinationAirportId = 1,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.UpdateFlight(flightToUpdate))
                .Throws<SameDepartureAndDestinationAirportsException>()
                .WithMessage(ErrorMessages.SAME_DEPARTURE_AND_DESTINATION);
        }

        [TestMethod]
        public void FlightServicesTests_UpdateFlight_ShouldThrowDepartureAirportNotFoundException()
        {
            // Arrange
            var flightToUpdate = new UpdateFlightArgs
            {
                Id = 1,
                FlightNumber = "FakeFlight_122",
                DepartureAirportId = 111111,
                DestinationAirportId = 1,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.UpdateFlight(flightToUpdate))
                .Throws<AirportNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.DEPARTURE_AIRPORT_NOT_FOUND, flightToUpdate.DepartureAirportId));
        }

        [TestMethod]
        public void FlightServicesTests_UpdateFlight_ShouldThrowDestinationAirportNotFoundException()
        {
            // Arrange
            var flightToUpdate = new UpdateFlightArgs
            {
                Id = 1,
                FlightNumber = "FakeFlight_30",
                DepartureAirportId = 1,
                DestinationAirportId = 232323,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.UpdateFlight(flightToUpdate))
                .Throws<AirportNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.DESTINATION_AIRPORT_NOT_FOUND, flightToUpdate.DestinationAirportId));
        }

        [TestMethod]
        public void FlightServicesTests_UpdateFlight_ShouldThrowAircraftNotFoundException()
        {
            // Arrange
            var flightToUpdate = new UpdateFlightArgs
            {
                Id = 1,
                FlightNumber = "FakeFlight_30",
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                AircraftId = 134343,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.UpdateFlight(flightToUpdate))
                .Throws<AircraftNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.AIRCRAFT_NOT_FOUND, flightToUpdate.AircraftId));
        }

        [TestMethod]
        public void FlightServicesTests_UpdateFlight_ShouldThrowDuplicateFlightException()
        {
            // Arrange
            _flightRepositoryMock.Setup(repo => repo.Search(It.IsAny<Expression<Func<Flight, bool>>>()))
                .Returns(new List<Flight>
                {
                    _flightsRecordsMock.First()
                }.AsQueryable());

            var flightToUpdate = new UpdateFlightArgs
            {
                Id = 2,
                FlightNumber = "FakeFlight_1",
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            Check.ThatCode(async () => await flightServices.UpdateFlight(flightToUpdate))
                .Throws<DuplicateFlightException>()
                .WithMessage(ErrorMessages.DUPLICATED_FLIGHT_NUMBER);
        }

        #endregion

        #region Delete A Flight
        [TestMethod]
        public async Task FlightServicesTests_Delete_ShouldCallRepositoryDelete()
        {
            // Arrange
            var flightId = 1;

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act
            var deletedFlight = await flightServices.DeleteFlight(flightId);

            // Assert
            Check.That(deletedFlight).IsTrue();
            _flightRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void FlightServicesTests_Delete_ShouldThrowFlightNotFoundException()
        {
            // Arrange
            var flightId = 1134;

            var flightServices = new FlightServices(_flightRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _mapperWrapper);

            // Act && Assert
            Check.ThatCode(async () => await flightServices.DeleteFlight(flightId))
                .Throws<FlightNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.FLIGHT_NOT_FOUND, flightId));
        }
        #endregion
    }
}
