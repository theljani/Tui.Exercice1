using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TUI.Flights.Common.Entities;

namespace TUI.Flights.Infrastructure.Tests
{
    [TestClass]
    public class EFUnitOfWorkTestsInMemoryDbTests
    {
        DbContextOptionsBuilder<EFUnitOfWork> _contextBuilder;
        List<Flight> _flightsRecords;
        List<Airport> _airportsRecords;
        List<Aircraft> _aircraftsRecords;

        [TestInitialize]
        public void Init()
        {
            // Use inMemory database
            _contextBuilder = new DbContextOptionsBuilder<EFUnitOfWork>();
            _contextBuilder.UseInMemoryDatabase(databaseName: "UnitTestsDb");

            // Initialize database with fake data
            Seed();
        }

        #region Init DbContext Test
        [TestMethod]
        public void EFUnitOfWorkTests_ShouldInitializeUnitOfWork()
        {

            // Arrange
            var builder = _contextBuilder.Options;
            var dbContext = new EFUnitOfWork(builder);

            // Assert
            Check.That(dbContext).IsNotNull();
        }
        #endregion

        #region CreateSet Test
        [TestMethod]
        public void EFUnitOfWorkTests_CreateSet_ShouldInstanciateAndReturnFlightDbSet()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Act
            var flightsDbSet = context.CreateSet<Flight>();

            // Assert
            Check.That(flightsDbSet).IsNotNull();
            Check.That(flightsDbSet.Count()).IsEqualTo(_flightsRecords.Count);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_CreateSet_ShouldInstanciateAndReturnAircraftDbSet()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Act
            var aircraftsDbSet = context.CreateSet<Aircraft>();

            // Assert
            Check.That(aircraftsDbSet).IsNotNull();
            Check.That(aircraftsDbSet.Count()).IsEqualTo(_aircraftsRecords.Count);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_CreateSet_ShouldInstanciateAndReturnAirportDbSet()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Act
            var airportsDbset = context.CreateSet<Airport>();

            // Assert
            Check.That(airportsDbset).IsNotNull();
            Check.That(airportsDbset.Count()).IsEqualTo(_airportsRecords.Count);

        }
        #endregion

        #region Get Record By Id Tests
        [TestMethod]
        public void EFUnitOfWorkTests_GetFlight_ShouldReturnAFlightWhenFlightIdExistsInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Expected
            var expectedFlight = _flightsRecords.First();

            // Act
            var flight = context.CreateSet<Flight>().Find(expectedFlight.Id);

            // Assert
            Check.That(flight).IsNotNull();

            Check.That(flight.CreationDate).IsEqualTo(expectedFlight.CreationDate);
            Check.That(flight.Distance).IsEqualTo(expectedFlight.Distance);
            Check.That(flight.EstimatedArrivalTime).IsEqualTo(expectedFlight.EstimatedArrivalTime);
            Check.That(flight.EstimatedFlightDuration).IsEqualTo(expectedFlight.EstimatedFlightDuration);
            Check.That(flight.EstimatedFuelNeeded).IsEqualTo(expectedFlight.EstimatedFuelNeeded);
            Check.That(flight.FlightNumber).IsEqualTo(expectedFlight.FlightNumber);
            Check.That(flight.FlightDate).IsEqualTo(expectedFlight.FlightDate);
            Check.That(flight.FlightTime).IsEqualTo(expectedFlight.FlightTime);

            Check.That(flight.Departure).HasFieldsWithSameValues(expectedFlight.Departure);
            Check.That(flight.Destination).HasFieldsWithSameValues(expectedFlight.Destination);
            Check.That(flight.Aircraft).HasFieldsWithSameValues(expectedFlight.Aircraft);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_GetFlight_ShouldReturnNullWhenFlightIdDoesNotExistInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightId = 199777;

            // Act
            var flight = context.CreateSet<Flight>().Find(flightId);

            // Assert
            Check.That(flight).IsNull();
        }

        [TestMethod]
        public void EFUnitOfWorkTests_GetAirport_ShouldReturnAnAirportWhenAirportIdExistsInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Expected
            var expectedAirport = _airportsRecords.First();

            // Act
            var currentAirport = context.CreateSet<Airport>().Find(expectedAirport.Id);

            // Assert
            Check.That(currentAirport).IsNotNull();
            Check.That(currentAirport).HasFieldsWithSameValues(expectedAirport);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_GetAirport_ShouldReturnNullWhenAirportIdDoesNotExistInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var airportId = 19977777;

            // Act
            var airport = context.CreateSet<Airport>().Find(airportId);

            // Assert
            Check.That(airport).IsNull();
        }

        [TestMethod]
        public void EFUnitOfWorkTests_GetAircraft_ShouldReturnAnAircraftWhenAircraftIdExistsInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Expected
            var expectedAircraft = _aircraftsRecords.First();

            // Act
            var currentAircraft = context.CreateSet<Aircraft>().Find(expectedAircraft.Id);

            // Assert
            Check.That(currentAircraft).IsNotNull();
            Check.That(currentAircraft).HasFieldsWithSameValues(expectedAircraft);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_GetAircraft_ShouldReturnNullWhenAircraftIdDoesNotExistInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var aircraftId = 19977777;

            // Act
            var aircraft = context.CreateSet<Aircraft>().Find(aircraftId);

            // Assert
            Check.That(aircraft).IsNull();
        }

        #endregion

        #region Get Record By Id Asynchronousily Tests
        [TestMethod]
        public async Task EFUnitOfWorkTests_GetFlight_Asynchronously_ShouldReturnAFlightWhenFlightIdExistsInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Expected
            var expectedFlight = _flightsRecords.First();

            // Act
            var flight = await context.CreateSet<Flight>().FindAsync(expectedFlight.Id);

            // Assert
            Check.That(flight).IsNotNull();

            Check.That(flight.CreationDate).IsEqualTo(expectedFlight.CreationDate);
            Check.That(flight.Distance).IsEqualTo(expectedFlight.Distance);
            Check.That(flight.EstimatedArrivalTime).IsEqualTo(expectedFlight.EstimatedArrivalTime);
            Check.That(flight.EstimatedFlightDuration).IsEqualTo(expectedFlight.EstimatedFlightDuration);
            Check.That(flight.EstimatedFuelNeeded).IsEqualTo(expectedFlight.EstimatedFuelNeeded);
            Check.That(flight.FlightNumber).IsEqualTo(expectedFlight.FlightNumber);
            Check.That(flight.FlightDate).IsEqualTo(expectedFlight.FlightDate);
            Check.That(flight.FlightTime).IsEqualTo(expectedFlight.FlightTime);

            Check.That(flight.Departure).HasFieldsWithSameValues(expectedFlight.Departure);
            Check.That(flight.Destination).HasFieldsWithSameValues(expectedFlight.Destination);
            Check.That(flight.Aircraft).HasFieldsWithSameValues(expectedFlight.Aircraft);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_GetFlight_Asynchronously_ShouldReturnNullWhenFlightIdDoesNotExistInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightId = 199777;

            // Act
            var flight = await context.CreateSet<Flight>().FindAsync(flightId);

            // Assert
            Check.That(flight).IsNull();
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_GetAirport_Asynchronously_ShouldReturnAnAirportWhenAirportIdExistsInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Expected
            var expectedAirport = _airportsRecords.First();

            // Act
            var currentAirport = await context.CreateSet<Airport>().FindAsync(expectedAirport.Id);

            // Assert
            Check.That(currentAirport).IsNotNull();
            Check.That(currentAirport).HasFieldsWithSameValues(expectedAirport);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_GetAirport_Asynchronously_ShouldReturnNullWhenAirportIdDoesNotExistInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var airportId = 19977777;

            // Act
            var airport = await context.CreateSet<Airport>().FindAsync(airportId);

            // Assert
            Check.That(airport).IsNull();
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_GetAircraft_Asynchronously_ShouldReturnAnAircraftWhenAircraftIdExistsInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Expected
            var expectedAircraft = _aircraftsRecords.First();

            // Act
            var currentAircraft = await context.CreateSet<Aircraft>().FindAsync(expectedAircraft.Id);

            // Assert
            Check.That(currentAircraft).IsNotNull();
            Check.That(currentAircraft).HasFieldsWithSameValues(expectedAircraft);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_GetAircraft_Asynchronously_ShouldReturnNullWhenAircraftIdDoesNotExistInDatabase()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var aircraftId = 19977777;

            // Act
            var aircraft = await context.CreateSet<Aircraft>().FindAsync(aircraftId);

            // Assert
            Check.That(aircraft).IsNull();
        }

        #endregion

        #region Get All Records Tests
        [TestMethod]
        public void EFUnitOfWorkTests_ShouldReturnAllFlightsRecords()
        {

            // Act
            List<Flight> records = null;
            var context = new EFUnitOfWork(_contextBuilder.Options);
            records = context.CreateSet<Flight>().ToList();


            // Assert
            Check.That(records).IsNotNull();
            Check.That(records.Count).IsEqualTo(_flightsRecords.Count);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldReturnAllAirportsRecords()
        {

            // Act
            List<Airport> records = null;
            var context = new EFUnitOfWork(_contextBuilder.Options);

            records = context.CreateSet<Airport>().ToList();


            // Assert

            Check.That(records).IsNotNull();
            Check.That(records.Count).IsEqualTo(_airportsRecords.Count);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldReturnAllAircraftsRecords()
        {

            // Act
            List<Aircraft> records = null;
            var context = new EFUnitOfWork(_contextBuilder.Options);

            records = context.CreateSet<Aircraft>().ToList();


            // Assert
            Check.That(records).IsNotNull();
            Check.That(records.Count()).IsEqualTo(_aircraftsRecords.Count);
        }
        #endregion

        #region Get All Records Asynchronousily Tests
        [TestMethod]
        public async Task EFUnitOfWorkTests_ShouldReturnAllFlightsRecordsAsynchronousily()
        {

            // Act
            List<Flight> records = null;
            var context = new EFUnitOfWork(_contextBuilder.Options);

            records = await context.CreateSet<Flight>().ToListAsync();

            // Assert
            Check.That(records).IsNotNull();
            Check.That(records.Count).IsEqualTo(_flightsRecords.Count);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_ShouldReturnAllAirportsRecordsAsynchronousily()
        {

            // Act
            List<Airport> records = null;
            var context = new EFUnitOfWork(_contextBuilder.Options);

            records = await context.CreateSet<Airport>().ToListAsync();


            // Assert
            Check.That(records).IsNotNull();
            Check.That(records.Count).IsEqualTo(_airportsRecords.Count);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_ShouldReturnAllAircraftsRecordsAsynchronousily()
        {

            // Act
            List<Aircraft> records = null;
            var context = new EFUnitOfWork(_contextBuilder.Options);

            records = await context.CreateSet<Aircraft>().ToListAsync();


            // Assert
            Check.That(records).IsNotNull();
            Check.That(records.Count()).IsEqualTo(_aircraftsRecords.Count);
        }
        #endregion

        #region Search Records Tests
        [TestMethod]
        public void EFUnitOfWorkTests_ShouldFindOnlyOneFlightRecordByFlightNumber()
        {
            //Arrange && Expected
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var expectedFlight = _flightsRecords.First();

            Expression<Func<Flight, bool>> filter = (Flight f) =>
                                       (f.FlightNumber == expectedFlight.FlightNumber);

            // Act
            var foundFlights = context.CreateSet<Flight>().Where(filter).ToList();

            // Assert
            Check.That(foundFlights).IsNotNull();
            Check.That(foundFlights.Count()).Equals(1);
            Check.That(foundFlights.First().Id).Equals(expectedFlight.Id);
            Check.That(foundFlights.First().FlightNumber).Equals(expectedFlight.FlightNumber);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldNotFindAnyFlightsRecords()
        {
            //Arrange && Expected
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightNumber = "AnyFlight";

            Expression<Func<Flight, bool>> filter = (Flight f) =>
                                       (f.FlightNumber == flightNumber);

            // Act
            var foundFlights = context.CreateSet<Flight>().Where(filter).ToList();

            // Assert
            Check.That(foundFlights).IsNotNull();
            Check.That(foundFlights).IsEmpty();

        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldFindSomeAirportsRecords()
        {
            //Arrange && Expected
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var expectedAirport = _airportsRecords.First();

            Expression<Func<Airport, bool>> filter = (Airport a) =>
                                       (a.Name == expectedAirport.Name);

            // Act
            var foundAirports = context.CreateSet<Airport>().Where(filter).ToList();

            // Assert
            Check.That(foundAirports).IsNotNull();
            Check.That(foundAirports.Count()).Equals(1);
            Check.That(foundAirports.First().Id).Equals(expectedAirport.Id);
            Check.That(foundAirports.First().Name).Equals(expectedAirport.Name);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldNotFindAnyAirportsRecords()
        {

            //Arrange && Expected
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var airportName = "AnyAirport";

            Expression<Func<Airport, bool>> filter = (Airport a) =>
                                       (a.Name == airportName);

            // Act
            var foundAirports = context.CreateSet<Airport>().Where(filter).ToList();

            // Assert
            Check.That(foundAirports).IsNotNull();
            Check.That(foundAirports).IsEmpty();
        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldFindSomeAircraftsRecords()
        {
            //Arrange && Expected
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var expectedAircraft = _aircraftsRecords.First();

            Expression<Func<Aircraft, bool>> filter = (Aircraft a) =>
                                       (a.Code == expectedAircraft.Code);

            // Act
            var foundAircrafts = context.CreateSet<Aircraft>().Where(filter).ToList();

            // Assert
            Check.That(foundAircrafts).IsNotNull();
            Check.That(foundAircrafts.Count()).Equals(1);
            Check.That(foundAircrafts.First().Id).Equals(expectedAircraft.Id);
            Check.That(foundAircrafts.First().Code).Equals(expectedAircraft.Code);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_ShouldNotFindAnyAircraftsRecords()
        {
            //Arrange && Expected
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var aircraftCode = "AnyAircraft";

            Expression<Func<Aircraft, bool>> filter = (Aircraft a) =>
                                       (a.Code == aircraftCode);

            // Act
            var foundAircrafts = context.CreateSet<Aircraft>().Where(filter).ToList();

            // Assert
            Check.That(foundAircrafts).IsNotNull();
            Check.That(foundAircrafts).IsEmpty();
        }
        #endregion

        #region Search Records Asynchronously Tests

        #endregion

        #region Create New Record Tests
        [TestMethod]
        public void EFUnitOfWorkTests_Create_ShouldCreateNewAircraft()
        {
            // Arrange
            var random = new Random();
            Aircraft newAircraft = new Aircraft
            {
                Code = "FakeAircraft_11",
                MilesPerHour = random.Next(100, 300),
                TakeOffEffort = random.Next(30, 50),
                ConsumptionKgPerH = random.Next(300, 800)
            };

            // Act
            var context = new EFUnitOfWork(_contextBuilder.Options);

            context.CreateSet<Aircraft>().Add(newAircraft);
            context.Commit();

            Aircraft currentCreatedRecord = context.CreateSet<Aircraft>().Find(newAircraft.Id);

            // Assert
            Check.That(currentCreatedRecord.Code).IsEqualTo(newAircraft.Code);
            Check.That(currentCreatedRecord.MilesPerHour).IsEqualTo(newAircraft.MilesPerHour);
            Check.That(currentCreatedRecord.TakeOffEffort).IsEqualTo(newAircraft.TakeOffEffort);
            Check.That(currentCreatedRecord.ConsumptionKgPerH).IsEqualTo(newAircraft.ConsumptionKgPerH);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_Create_ShouldCreateNewAirport()
        {
            // Arrange
            var random = new Random();
            Airport newAirport = new Airport
            {
                Name = "FakeAirport_10",
                Latitude = random.Next(1000, 99999),
                Longitude = random.Next(1000, 99999)
            };
            // Act
            var context = new EFUnitOfWork(_contextBuilder.Options);

            context.CreateSet<Airport>().Add(newAirport);
            context.Commit();

            Airport currentCreatedRecord = context.CreateSet<Airport>().Find(newAirport.Id);

            // Assert
            Check.That(currentCreatedRecord.Name).IsEqualTo(newAirport.Name);
            Check.That(currentCreatedRecord.Latitude).IsEqualTo(newAirport.Latitude);
            Check.That(currentCreatedRecord.Longitude).IsEqualTo(newAirport.Longitude);
        }

        [TestMethod]
        public void EFUnitOfWorkTests_Create_ShouldCreateNewFlight()
        {
            // Arrange
            Flight newFlight = new Flight
            {
                FlightNumber = "FakeFlight_" + 10,
                AirportDepartureId = 1,
                AirportDestinationId = 2,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };
            // Act
            var context = new EFUnitOfWork(_contextBuilder.Options);

            context.CreateSet<Flight>().Add(newFlight);
            context.Commit();

            Flight currentCreatedRecord = context.CreateSet<Flight>().Find(newFlight.Id);

            // Assert
            Check.That(currentCreatedRecord.FlightNumber).IsEqualTo(newFlight.FlightNumber);
            Check.That(currentCreatedRecord.AirportDepartureId).IsEqualTo(newFlight.AirportDepartureId);
            Check.That(currentCreatedRecord.AirportDestinationId).IsEqualTo(newFlight.AirportDestinationId);
            Check.That(currentCreatedRecord.AircraftId).IsEqualTo(newFlight.AircraftId);
            Check.That(currentCreatedRecord.FlightDate).IsEqualTo(newFlight.FlightDate);
            Check.That(currentCreatedRecord.FlightTime).IsEqualTo(newFlight.FlightTime);
        }
        #endregion

        #region Create New Record Asynchronousily Tests
        [TestMethod]
        public async Task EFUnitOfWorkTests_ShouldCreateNewAircraftAsynchronousily()
        {
            // Arrange
            var random = new Random();
            Aircraft newAircraft = new Aircraft
            {
                Code = "FakeAircraft_11",
                MilesPerHour = random.Next(100, 300),
                TakeOffEffort = random.Next(30, 50),
                ConsumptionKgPerH = random.Next(300, 800)
            };

            // Act
            var context = new EFUnitOfWork(_contextBuilder.Options);

            context.CreateSet<Aircraft>().Add(newAircraft);
            await context.CommitAsync();

            Aircraft currentCreatedRecord = context.CreateSet<Aircraft>().Find(newAircraft.Id);

            // Assert
            Check.That(currentCreatedRecord.Code).IsEqualTo(newAircraft.Code);
            Check.That(currentCreatedRecord.MilesPerHour).IsEqualTo(newAircraft.MilesPerHour);
            Check.That(currentCreatedRecord.TakeOffEffort).IsEqualTo(newAircraft.TakeOffEffort);
            Check.That(currentCreatedRecord.ConsumptionKgPerH).IsEqualTo(newAircraft.ConsumptionKgPerH);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_ShouldCreateNewAirportAsynchronousily()
        {
            // Arrange
            var random = new Random();
            Airport newAirport = new Airport
            {
                Name = "FakeAirport_10",
                Latitude = random.Next(1000, 99999),
                Longitude = random.Next(1000, 99999)
            };
            // Act
            var context = new EFUnitOfWork(_contextBuilder.Options);
            context.CreateSet<Airport>().Add(newAirport);
            await context.CommitAsync();

            Airport currentCreatedRecord = context.CreateSet<Airport>().Find(newAirport.Id);

            // Assert
            Check.That(currentCreatedRecord.Name).IsEqualTo(newAirport.Name);
            Check.That(currentCreatedRecord.Latitude).IsEqualTo(newAirport.Latitude);
            Check.That(currentCreatedRecord.Longitude).IsEqualTo(newAirport.Longitude);
        }

        [TestMethod]
        public async Task EFUnitOfWorkTests_ShouldCreateNewFlightAsynchronousily()
        {
            // Arrange
            Flight newFlight = new Flight
            {
                FlightNumber = "FakeFlight_" + 10,
                AirportDepartureId = 1,
                AirportDestinationId = 2,
                AircraftId = 1,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };
            // Act
            var context = new EFUnitOfWork(_contextBuilder.Options);

            context.CreateSet<Flight>().Add(newFlight);
            await context.CommitAsync();


            Flight currentCreatedRecord = context.CreateSet<Flight>().Find(newFlight.Id);


            // Assert
            Check.That(currentCreatedRecord.FlightNumber).IsEqualTo(newFlight.FlightNumber);
            Check.That(currentCreatedRecord.AirportDepartureId).IsEqualTo(newFlight.AirportDepartureId);
            Check.That(currentCreatedRecord.AirportDestinationId).IsEqualTo(newFlight.AirportDestinationId);
            Check.That(currentCreatedRecord.AircraftId).IsEqualTo(newFlight.AircraftId);
            Check.That(currentCreatedRecord.FlightDate).IsEqualTo(newFlight.FlightDate);
            Check.That(currentCreatedRecord.FlightTime).IsEqualTo(newFlight.FlightTime);
        }
        #endregion

        #region Update A Record Tests
        [TestMethod]
        public void EFUnitOfWorkTests_Update_ShouldUpdateAnExistingFlight()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightToUpdate = context.CreateSet<Flight>().First();
            flightToUpdate.FlightNumber = "NewFlightNumber";
            flightToUpdate.AirportDepartureId = 4;
            flightToUpdate.AirportDestinationId = 5;
            flightToUpdate.AircraftId = 2;

            // Expected
            var expectedFlight = context.CreateSet<Flight>().First();

            // Act
            var currentEntity = context.CreateSet<Flight>().Update(flightToUpdate).Entity;
            context.SaveChanges();

            // Assert
            Check.That(currentEntity).IsSameReferenceAs(expectedFlight);
            Check.That(currentEntity).HasSameValueAs(expectedFlight);
        }
        #endregion

        #region Update A Record Asynchronously Tests
        [TestMethod]
        public async Task EFUnitOfWorkTests_Update_Asynchronousily_ShouldUpdateAnExistingFlight()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightToUpdate = context.CreateSet<Flight>().First();
            flightToUpdate.FlightNumber = "NewFlightNumber";
            flightToUpdate.AirportDepartureId = 4;
            flightToUpdate.AirportDestinationId = 5;
            flightToUpdate.AircraftId = 2;

            // Expected
            var expectedFlight = context.CreateSet<Flight>().First();

            // Act
            var currentEntity = context.CreateSet<Flight>().Update(flightToUpdate).Entity;
            await context.SaveChangesAsync();

            // Assert
            Check.That(currentEntity).IsSameReferenceAs(expectedFlight);
            Check.That(currentEntity).HasSameValueAs(expectedFlight);
        }
        #endregion

        #region Delete A Record Tests
        [TestMethod]
        public void EFUnitOfWorkTests_Delete_ShouldDeleteAnExistingFlight()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightToDelete = context.CreateSet<Flight>().Last();


            // Expected

            // Act
            EntityEntry<Flight> deletedFlight = context.CreateSet<Flight>().Remove(flightToDelete);
            context.SaveChanges();
            var exists = context.CreateSet<Flight>().Where(e => e.Id.Equals(flightToDelete.Id)).Any();

            // Assert
            Check.That(deletedFlight.State).IsEqualTo(EntityState.Detached);
            Check.That(exists).IsFalse();
        }
        #endregion

        #region Delete A Record Asynchronously Tests
        [TestMethod]
        public async Task EFUnitOfWorkTests_Delete_Asynchronousily_ShouldDeleteAnExistingFlight()
        {
            // Arrange
            var context = new EFUnitOfWork(_contextBuilder.Options);
            var flightToDelete = context.CreateSet<Flight>().Last();

            // Expected

            // Act
            EntityEntry<Flight> deletedFlight = context.CreateSet<Flight>().Remove(flightToDelete);
            await context.SaveChangesAsync();
            var exists = context.CreateSet<Flight>().Where(e => e.Id.Equals(flightToDelete.Id)).Any();

            // Assert
            Check.That(deletedFlight.State).IsEqualTo(EntityState.Detached);
            Check.That(exists).IsFalse();

        }
        #endregion

        private void Seed()
        {
            var context = new EFUnitOfWork(_contextBuilder.Options);

            // Ensure to clear data if exists
            if (context.CreateSet<Flight>().Count() > 0)
            {

                context.CreateSet<Flight>().ToList().ForEach(record =>
                {
                    context.Remove(record);
                });
            }

            if (context.CreateSet<Aircraft>().Count() > 0)
            {

                context.CreateSet<Aircraft>().ToList().ForEach(record =>
                {
                    context.Remove(record);
                });
            }

            if (context.CreateSet<Airport>().Count() > 0)
            {

                context.CreateSet<Airport>().ToList().ForEach(record =>
                {
                    context.Remove(record);
                });
            }
            // Create Fake Records

            Random random = new Random();
            _airportsRecords = new List<Airport>();
            _aircraftsRecords = new List<Aircraft>();
            _flightsRecords = new List<Flight>();

            for (int i = 1; i < 10; i++)
            {
                _airportsRecords.Add(new Airport
                {
                    Name = "FakeAirport_" + i,
                    Latitude = random.Next(1000, 99999),
                    Longitude = random.Next(1000, 99999)
                });
            }

            for (int i = 1; i < 10; i++)
            {
                _aircraftsRecords.Add(new Aircraft
                {
                    Code = "FakeAircraft_" + i,
                    MilesPerHour = random.Next(100, 300),
                    TakeOffEffort = random.Next(30, 50),
                    ConsumptionKgPerH = random.Next(300, 800)
                });
            }

            for (int i = 1; i < 10; i++)
            {
                _flightsRecords.Add(new Flight
                {
                    FlightNumber = "FakeFlight_" + i,
                    AirportDepartureId = i,
                    AirportDestinationId = i + 1,
                    AircraftId = i,
                    FlightDate = DateTime.Now,
                    FlightTime = DateTime.Now
                });
            }

            context.AddRange(_airportsRecords);
            context.AddRange(_aircraftsRecords);
            context.AddRange(_flightsRecords);
            context.SaveChanges();
        }
    }
}
