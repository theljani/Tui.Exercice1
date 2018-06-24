using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Entities;
using TUI.Flights.Common.Entities.Base;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Infrastructure.Tests
{
    [TestClass]
    public class RepositoryTests : DbContext
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<DbSet<Flight>> _flightDbSetMock;
        private Mock<DbSet<Airport>> _airportDbSetMock;
        private Mock<DbSet<Aircraft>> _aircraftDbSetMock;
   
        private Repository<Flight> _flightsRepository;
        private Repository<Airport> _airportsRepository;
        private Repository<Aircraft> _aircraftsRepository;

        List<Flight> _flightsRecords;
        List<Airport> _airportsRecords;
        List<Aircraft> _aircraftsRecords;

        [TestInitialize]
        public void Init()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            //_flightEntityEntryMock = new Mock<EntityEntry<Flight>>();

            InitializeAndSetupDbSetMocks();
            InitializeAndSetupUnitOfWork();

            _flightsRepository = new Repository<Flight>(_unitOfWorkMock.Object);
            _airportsRepository = new Repository<Airport>(_unitOfWorkMock.Object);
            _aircraftsRepository = new Repository<Aircraft>(_unitOfWorkMock.Object);


        }

        #region Get Record

        #region Flight
        [TestMethod]
        public void RepositoryTests_Get_ShouldReturnAFlightById()
        {
            // Arrange
            int flightId = _flightsRecords.First().Id;

            // Expected 
            var expectedFlight = _flightsRecords.First();

            // Act 
            var currentFlight = _flightsRepository.Get(flightId);

            // Assert
            Check.That(currentFlight).IsNotNull();
            Check.That(currentFlight).IsInstanceOf<Flight>();
            Check.That(currentFlight).IsSameReferenceAs(expectedFlight);
            Check.That(currentFlight).HasSameValueAs(expectedFlight);

        }

        [TestMethod]
        public async Task RepositoryTests_Get_Asynchronousily_ShouldReturnAFlightById()
        {
            // Arrange
            int flightId = _flightsRecords.First().Id;

            // Expected 
            var expectedFlight = _flightsRecords.First();

            // Act 
            var currentFlight = await _flightsRepository.GetAsync(flightId);

            // Asserts
            Check.That(currentFlight).IsNotNull();
            Check.That(currentFlight).IsInstanceOf<Flight>();
            Check.That(currentFlight).IsSameReferenceAs(expectedFlight);
            Check.That(currentFlight).HasSameValueAs(expectedFlight);

        }
        #endregion

        #region Airport
        [TestMethod]
        public void RepositoryTests_Get_ShouldReturnAnAirportById()
        {
            // Arrange
            int airportId = _airportsRecords.First().Id;

            // Expected 
            var expectedAirport = _airportsRecords.First();

            // Act 
            var currentAirport = _airportsRepository.Get(airportId);

            // Assert
            Check.That(currentAirport).IsNotNull();
            Check.That(currentAirport).IsInstanceOf<Airport>();
            Check.That(currentAirport).IsSameReferenceAs(expectedAirport);
            Check.That(currentAirport).HasSameValueAs(expectedAirport);
        }

        [TestMethod]
        public async Task RepositoryTests_Get_Asynchronousily_ShouldReturnAnAirportById()
        {
            // Arrange
            int airportId = _airportsRecords.First().Id;

            // Expected 
            var expectedAirport = _airportsRecords.First();

            // Act 
            var currentAirport = await _airportsRepository.GetAsync(airportId);

            // Assert
            Check.That(currentAirport).IsNotNull();
            Check.That(currentAirport).IsInstanceOf<Airport>();
            Check.That(currentAirport).IsSameReferenceAs(expectedAirport);
            Check.That(currentAirport).HasSameValueAs(expectedAirport);
        }
        #endregion

        #region Aircraft
        [TestMethod]
        public void RepositoryTests_Get_ShouldReturnAnAircraftById()
        {
            // Arrange
            int aircraftId = _aircraftsRecords.First().Id;

            // Expected 
            var expectedAircraft = _aircraftsRecords.First();

            // Act 
            var currentAircraft = _aircraftsRepository.Get(aircraftId);

            // Assert
            Check.That(currentAircraft).IsNotNull();
            Check.That(currentAircraft).IsInstanceOf<Aircraft>();
            Check.That(currentAircraft).IsSameReferenceAs(expectedAircraft);
            Check.That(currentAircraft).HasSameValueAs(expectedAircraft);
        }

        [TestMethod]
        public async Task RepositoryTests_Get_Asynchronousily_ShouldReturnAnAircraftById()
        {
            // Arrange
            int aircraftId = _aircraftsRecords.First().Id;

            // Expected 
            var expectedAircraft = _aircraftsRecords.First();

            // Act 
            var currentAircraft = await _aircraftsRepository.GetAsync(aircraftId);

            // Assert
            Check.That(currentAircraft).IsNotNull();
            Check.That(currentAircraft).IsInstanceOf<Aircraft>();
            Check.That(currentAircraft).IsSameReferenceAs(expectedAircraft);
            Check.That(currentAircraft).HasSameValueAs(expectedAircraft);
        }
        #endregion

        #endregion

        #region Get All Records

        #region Flight
        [TestMethod]
        public void RepositoryTests_GetAll_ShouldReturnAllFlights()
        {

            // Expected 
            var expectedFlights = _flightsRecords;

            // Act 
            var currentFlights = _flightsRepository.GetAll();

            // Assert
            Check.That(currentFlights).IsNotNull();
            Check.That(currentFlights.ToList().Count()).IsEqualTo(expectedFlights.Count());
            Check.That(currentFlights.ToList()).ContainsExactly(expectedFlights);

        }

        #endregion

        #region Airport
        [TestMethod]
        public void RepositoryTests_GetAll_ShouldReturnAllAirports()
        {
            // Expected 
            var expectedAirports = _airportsRecords;

            // Act 
            var currentAirports = _airportsRepository.GetAll();

            // Assert
            Check.That(currentAirports).IsNotNull();
            Check.That(currentAirports.ToList().Count()).IsEqualTo(expectedAirports.Count());
            Check.That(currentAirports.ToList()).ContainsExactly(expectedAirports);
        }
        #endregion

        #region Aircraft
        [TestMethod]
        public void RepositoryTests_GetAll_ShouldReturnAllAircrafts()
        {
            // Expected 
            var expectedAircrafts = _aircraftsRecords;

            // Act 
            var currentAircrafts = _aircraftsRepository.GetAll();

            // Assert
            Check.That(currentAircrafts).IsNotNull();
            Check.That(currentAircrafts.ToList().Count()).IsEqualTo(expectedAircrafts.Count());
            Check.That(currentAircrafts.ToList()).ContainsExactly(expectedAircrafts);
        }
        #endregion

        #endregion


        #region Create Record

        #endregion

        private void InitializeAndSetupUnitOfWork()
        {
            _unitOfWorkMock.Setup(u => u.Commit())
            .Callback(() => { });

            _unitOfWorkMock.Setup(u => u.CommitAsync())
                .Callback(() => { });

            _unitOfWorkMock.Setup(u => u.CreateSet<Flight>())
                .Returns(_flightDbSetMock.Object);
            _unitOfWorkMock.Setup(u => u.Flights)
                .Returns(_flightDbSetMock.Object);

            _unitOfWorkMock.Setup(u => u.CreateSet<Aircraft>())
                .Returns(_aircraftDbSetMock.Object);
            _unitOfWorkMock.Setup(u => u.Aircrafts)
                .Returns(_aircraftDbSetMock.Object);

            _unitOfWorkMock.Setup(u => u.CreateSet<Airport>())
                .Returns(_airportDbSetMock.Object);
            _unitOfWorkMock.Setup(u => u.Airports)
                .Returns(_airportDbSetMock.Object);

            _unitOfWorkMock.Setup(u => u.GetEntry<Flight>(It.IsAny<Flight>()))
                .Callback(() => { });

        }

        private void InitializeAndSetupDbSetMocks()
        {
            var random = new Random();
            _flightsRecords = new List<Flight>();

            for (int i = 1; i < 10; i++)
            {
                _flightsRecords.Add(new Flight
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

            _airportsRecords = new List<Airport>();
            for (int i = 1; i < 10; i++)
            {
                _airportsRecords.Add(new Airport
                {
                    Id = i,
                    Name = "FakeAirport_" + i,
                    Latitude = random.Next(-90, 90),
                    Longitude = random.Next(-90, 90)
                });
            }

            _aircraftsRecords = new List<Aircraft>();
            for (int i = 1; i < 10; i++)
            {
                _aircraftsRecords.Add(new Aircraft
                {
                    Id = i,
                    Code = "FakeAircraft_" + i,
                    MilesPerHour = random.Next(100, 300),
                    TakeOffEffort = random.Next(30, 50),
                    ConsumptionKgPerH = random.Next(300, 800)
                });
            }

            #region Flights DbSet Setup
            _flightDbSetMock = new Mock<DbSet<Flight>>();
            _flightDbSetMock.As<IEnumerable<Flight>>()
                .Setup(fDbset => fDbset.GetEnumerator()).Returns(() => _flightsRecords.GetEnumerator());


            _flightDbSetMock.As<IQueryable<Flight>>()
                .Setup(fDbset => fDbset.GetEnumerator()).Returns(() => _flightsRecords.GetEnumerator());

            _flightDbSetMock.Setup(fDbset => fDbset.Find(It.IsAny<object[]>()))
                .Returns((object[] flightIds) =>
                {
                    return _flightsRecords.Where(f => f.Id.Equals(flightIds[0])).FirstOrDefault();
                });

            _flightDbSetMock.Setup(fDbset => fDbset.FindAsync(It.IsAny<object[]>()))
                .Returns(async (object[] flightIds) =>
                {
                    return await _flightsRecords.ToAsyncEnumerable().Where(f => f.Id.Equals(flightIds[0])).FirstOrDefault();
                });

            _flightDbSetMock.Setup(fDbSet => fDbSet.Add(It.IsAny<Flight>()))
                .Callback((Flight newFlight) =>
                {
                    _flightsRecords.Add(newFlight);
                });
            //_flightDbSetMock.Setup(fDbSet => fDbSet.Add(It.IsAny<Flight>()).Entity)
            //.Returns(new Flight());
            #endregion

            #region Aircrafts DbSet Setup
            _aircraftDbSetMock = new Mock<DbSet<Aircraft>>();
            _aircraftDbSetMock.As<IEnumerable<Aircraft>>()
                .Setup(acDbset => acDbset.GetEnumerator()).Returns(() => _aircraftsRecords.GetEnumerator());
            _aircraftDbSetMock.As<IQueryable<Aircraft>>()
                .Setup(acDbset => acDbset.GetEnumerator()).Returns(() => _aircraftsRecords.GetEnumerator());

            _aircraftDbSetMock.Setup(acDbset => acDbset.Find(It.IsAny<object[]>()))
                .Returns((object[] aircraftsIds) =>
                {
                    return _aircraftsRecords.Where(f => f.Id.Equals(aircraftsIds[0])).FirstOrDefault();
                });
            _aircraftDbSetMock.Setup(acDbset => acDbset.FindAsync(It.IsAny<object[]>()))
                .Returns(async (object[] aircraftsIds) =>
                {
                    return await _aircraftsRecords.ToAsyncEnumerable().Where(f => f.Id.Equals(aircraftsIds[0])).FirstOrDefault();
                });
            #endregion

            #region Airports DbSet Setup
            _airportDbSetMock = new Mock<DbSet<Airport>>();
            _airportDbSetMock.As<IEnumerable<Airport>>()
                .Setup(arDbset => arDbset.GetEnumerator()).Returns(() => _airportsRecords.GetEnumerator());
            _airportDbSetMock.Setup(arDbset => arDbset.Find(It.IsAny<object[]>()))
                .Returns((object[] airportsIds) =>
                {
                    return _airportsRecords.Where(f => f.Id.Equals(airportsIds[0])).FirstOrDefault();
                });

            _airportDbSetMock.Setup(arDbset => arDbset.FindAsync(It.IsAny<object[]>()))
                .Returns(async (object[] airportsIds) =>
                {
                    return await _airportsRecords.ToAsyncEnumerable().Where(f => f.Id.Equals(airportsIds[0])).FirstOrDefault();
                });
            #endregion
        }
    }
}
