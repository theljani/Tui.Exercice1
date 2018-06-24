//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TUI.Flights.Common.Entities;
//using TUI.Flights.Common.ValueObjects;

//namespace TUI.Flights.Infrastructure.Tests
//{
//    [TestClass]
//    public class EFUnitOfWorkRelationalDbTests : IDisposable
//    {
//        EFUnitOfWork _context;
//        DbContextOptionsBuilder<EFUnitOfWork> _builder;
//        List<Flight> _flightsRecords;
//        List<Airport> _airportsRecords;
//        List<Aircraft> _aircraftsRecords;

//        [TestInitialize]
//        public void Init()
//        {
//            // Use relational database
//            var serviceProvider = new ServiceCollection()
//                .AddEntityFrameworkSqlServer()
//                .AddEntityFrameworkProxies()
//                .BuildServiceProvider();

//            _builder = new DbContextOptionsBuilder<EFUnitOfWork>();
//            _builder
//                .UseLazyLoadingProxies()
//                .UseSqlServer($"Server= (localdb)\\mssqllocaldb; Database = TUIFlightsDBForUnitTests_{Guid.NewGuid()};Integrated Security = True")
//                    .UseInternalServiceProvider(serviceProvider)
//                    .EnableSensitiveDataLogging();

//            _context = new EFUnitOfWork(_builder.Options);
            
//            _context.Database.EnsureDeleted();
//            _context.Database.EnsureCreated();
//            _context.Database.Migrate();

//            // Initialize database with fake data
//            //Seed();
//        }

//        #region Init DbContext Test
//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_ShouldInitializeUnitOfWork()
//        {

//            // Arrange
//            //var builder = _builder.Options;
//            var dbContext = new EFUnitOfWork(_builder.Options);

//            // Assert
//            Assert.IsNotNull(dbContext);
//        }

//        public void Dispose()
//        {
//            _context.Database.EnsureDeleted();
//        }

//        #endregion

//        #region Get Record By Id


//        #endregion

//        #region Get All Records Tests
//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_ShouldReturnAllFlightsRecords()
//        {

//            // Act
//            List<Flight> records = _context.Flights.ToList();

//            // Assert
//            Assert.IsNotNull(records);
//            Assert.AreEqual(records.Count, _flightsRecords.Count);
//        }

//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_ShouldReturnAllAirportsRecords()
//        {

//            // Act
//            List<Airport> records = _context.Airports.ToList();

//            // Assert
//            Assert.IsNotNull(records);
//            Assert.AreEqual(records.Count, _airportsRecords.Count);
//        }

//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_ShouldReturnAllAircraftsRecords()
//        {

//            // Act
//            List<Aircraft> records = records = _context.Aircrafts.ToList();

//            // Assert
//            Assert.IsNotNull(records);
//            Assert.AreEqual(records.Count, _aircraftsRecords.Count);
//        }
//        #endregion

//        #region Create New Record Tests
//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_Commit_ShouldCreateNewAircraft()
//        {
//            // Arrange
//            var random = new Random();
//            Aircraft newAircraft = new Aircraft
//            {
//                Code = "FakeAircraft_11",
//                MilesPerHour = random.Next(100, 300),
//                TakeOffEffort = random.Next(30, 50),
//                ConsumptionKgPerH = random.Next(300, 800)
//            };

//            // Act
//            _context.Aircrafts.Add(newAircraft);
//            _context.Commit();

//            Aircraft currentCreatedRecord = _context.Aircrafts.Find(newAircraft.Id);

//            // Assert
//            Assert.AreEqual(currentCreatedRecord.Code, newAircraft.Code);
//            Assert.AreEqual(currentCreatedRecord.MilesPerHour, newAircraft.MilesPerHour);
//            Assert.AreEqual(currentCreatedRecord.TakeOffEffort, newAircraft.TakeOffEffort);
//            Assert.AreEqual(currentCreatedRecord.ConsumptionKgPerH, newAircraft.ConsumptionKgPerH);
//        }

//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_Commit_ShouldCreateNewAirport()
//        {
//            // Arrange
//            var random = new Random();
//            Airport newAirport = new Airport
//            {
//                Name = "FakeAirport_10",
//                Latitude = random.Next(1000, 99999),
//                Longitude = random.Next(1000, 99999)
//            };

//            // Act
//            _context.Airports.Add(newAirport);
//            _context.Commit();

//            Airport currentCreatedRecord = _context.Airports.Find(newAirport.Id);

//            // Assert
//            Assert.AreEqual(currentCreatedRecord.Name, newAirport.Name);
//            Assert.AreEqual(currentCreatedRecord.Latitude, newAirport.Latitude);
//            Assert.AreEqual(currentCreatedRecord.Longitude, newAirport.Longitude);
//        }

//        [TestMethod]
//        public void EFUnitOfWorkRelationalDbTests_Commit_ShouldCreateNewFlight()
//        {
//            // Arrange
//            Flight newFlight = new Flight
//            {
//                FlightNumber = "FakeFlight_" + 10,
//                AirportDepartureId = 1,
//                AirportDestinationId = 2,
//                AircraftId = 1,
//                FlightDate = DateTime.Now,
//                FlightTime = DateTime.Now
//            };
//            // Act
//            _context.Flights.Add(newFlight);
//            _context.Commit();

//            Flight currentCreatedRecord = _context.Flights.Find(newFlight.Id);

//            // Assert
//            Assert.AreEqual(currentCreatedRecord.FlightNumber, newFlight.FlightNumber);
//            Assert.AreEqual(currentCreatedRecord.AirportDepartureId, newFlight.AirportDepartureId);
//            Assert.AreEqual(currentCreatedRecord.AirportDestinationId, newFlight.AirportDestinationId);
//            Assert.AreEqual(currentCreatedRecord.AircraftId, newFlight.AircraftId);
//            Assert.AreEqual(currentCreatedRecord.FlightDate, newFlight.FlightDate);
//            Assert.AreEqual(currentCreatedRecord.FlightTime, newFlight.FlightTime);
//        }
//        #endregion

//        #region Create New Record Asynchronousily Tests
//        [TestMethod]
//        public async Task EFUnitOfWorkRelationalDbTests_ShouldCreateNewAircraftAsynchronousily()
//        {
//            // Arrange
//            var random = new Random();
//            Aircraft newAircraft = new Aircraft
//            {
//                Code = "FakeAircraft_11",
//                MilesPerHour = random.Next(100, 300),
//                TakeOffEffort = random.Next(30, 50),
//                ConsumptionKgPerH = random.Next(300, 800)
//            };

//            // Act
//            _context.Aircrafts.Add(newAircraft);
//            await _context.CommitAsync();


//            Aircraft currentCreatedRecord = _context.Aircrafts.Find(newAircraft.Id);


//            // Assert
//            Assert.AreEqual(currentCreatedRecord.Code, newAircraft.Code);
//            Assert.AreEqual(currentCreatedRecord.MilesPerHour, newAircraft.MilesPerHour);
//            Assert.AreEqual(currentCreatedRecord.TakeOffEffort, newAircraft.TakeOffEffort);
//            Assert.AreEqual(currentCreatedRecord.ConsumptionKgPerH, newAircraft.ConsumptionKgPerH);
//        }

//        [TestMethod]
//        public async Task EFUnitOfWorkRelationalDbTests_ShouldCreateNewAirportAsynchronousily()
//        {
//            // Arrange
//            var random = new Random();
//            Airport newAirport = new Airport
//            {
//                Name = "FakeAirport_10",
//                Latitude = random.Next(1000, 99999),
//                Longitude = random.Next(1000, 99999)
//            };
//            // Act
//            _context.Airports.Add(newAirport);
//            await _context.CommitAsync();

//            Airport currentCreatedRecord = _context.Airports.Find(newAirport.Id);


//            // Assert
//            Assert.AreEqual(currentCreatedRecord.Name, newAirport.Name);
//            Assert.AreEqual(currentCreatedRecord.Latitude, newAirport.Latitude);
//            Assert.AreEqual(currentCreatedRecord.Longitude, newAirport.Longitude);
//        }

//        [TestMethod]
//        public async Task EFUnitOfWorkRelationalDbTests_ShouldCreateNewFlightAsynchronousily()
//        {
//            // Arrange
//            Flight newFlight = new Flight
//            {
//                FlightNumber = "FakeFlight_" + 10,
//                AirportDepartureId = 1,
//                AirportDestinationId = 2,
//                AircraftId = 1,
//                FlightDate = DateTime.Now,
//                FlightTime = DateTime.Now
//            };
//            // Act
//            _context.Flights.Add(newFlight);
//            await _context.CommitAsync();


//            Flight currentCreatedRecord = _context.Flights.Find(newFlight.Id);

//            // Assert
//            Assert.AreEqual(currentCreatedRecord.FlightNumber, newFlight.FlightNumber);
//            Assert.AreEqual(currentCreatedRecord.AirportDepartureId, newFlight.AirportDepartureId);
//            Assert.AreEqual(currentCreatedRecord.AirportDestinationId, newFlight.AirportDestinationId);
//            Assert.AreEqual(currentCreatedRecord.AircraftId, newFlight.AircraftId);
//            Assert.AreEqual(currentCreatedRecord.FlightDate, newFlight.FlightDate);
//            Assert.AreEqual(currentCreatedRecord.FlightTime, newFlight.FlightTime);
//        }
//        #endregion

//        private void Seed()
//        {
//            using (var dbContext = new EFUnitOfWork(_builder.Options))
//            {
//                // Ensure to clear data if exists
//                if (dbContext.Flights.Count() > 0)
//                {

//                    dbContext.Flights.ToList().ForEach(record =>
//                    {
//                        dbContext.Remove(record);
//                    });
//                }

//                if (dbContext.Aircrafts.Count() > 0)
//                {

//                    dbContext.Aircrafts.ToList().ForEach(record =>
//                    {
//                        dbContext.Remove(record);
//                    });
//                }

//                if (dbContext.Airports.Count() > 0)
//                {

//                    dbContext.Airports.ToList().ForEach(record =>
//                    {
//                        dbContext.Remove(record);
//                    });
//                }
//                // Create Fake Records

//                Random random = new Random();
//                _airportsRecords = new List<Airport>();
//                _aircraftsRecords = new List<Aircraft>();
//                _flightsRecords = new List<Flight>();

//                for (int i = 1; i < 10; i++)
//                {
//                    _airportsRecords.Add(new Airport
//                    {
//                        Name = "FakeAirport_" + i,
//                        Latitude = random.Next(1000, 99999),
//                        Longitude = random.Next(1000, 99999)
//                    });
//                }

//                for (int i = 1; i < 10; i++)
//                {
//                    _aircraftsRecords.Add(new Aircraft
//                    {
//                        Code = "FakeAircraft_" + i,
//                        MilesPerHour = random.Next(100, 300),
//                        TakeOffEffort = random.Next(30, 50),
//                        ConsumptionKgPerH = random.Next(300, 800)
//                    });
//                }

//                for (int i = 1; i < 10; i++)
//                {
//                    _flightsRecords.Add(new Flight
//                    {
//                        FlightNumber = "FakeFlight_" + i,
//                        AirportDepartureId = i,
//                        AirportDestinationId = i + 1,
//                        AircraftId = i,
//                        FlightDate = DateTime.Now,
//                        FlightTime = DateTime.Now
//                    });
//                }

//                dbContext.AddRange(_airportsRecords);
//                dbContext.AddRange(_aircraftsRecords);
//                dbContext.AddRange(_flightsRecords);
//                dbContext.SaveChanges();
//            }

//        }
//    }
//}
