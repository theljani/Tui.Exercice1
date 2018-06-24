using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Args.Flight;
using TUI.Flights.Common.Dtos.Aircraft;
using TUI.Flights.Common.Dtos.Airport;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Core.Services.AircraftServices;
using TUI.Flights.Core.Services.AirportServices;
using TUI.Flights.Core.Services.FlightServices;
using System;
using TUI.Flights.Web.Controllers;
using NFluent;
using Microsoft.AspNetCore.Mvc;
using TUI.Flights.Common.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TUI.Flights.Common.Exceptions;
using TUI.Flights.Core.Helpers;

namespace TUI.Flights.Web.Tests
{
    [TestClass]
    public class FlightControllerTests
    {
        private Mock<IFlightServices> _flightCoreServicesMock;
        private Mock<IAirportServices> _airportCoreServicesMock;
        private Mock<IAircraftServices> _aircraftCoreServicesMock;

        private FlightListDto _flightsMockData;
        private List<AirportDto> _airportsMockData;
        private List<AircraftDto> _aircraftsMockData;

        [TestInitialize]
        public void Init()
        {
            InitMockData();

            _flightCoreServicesMock = new Mock<IFlightServices>();
            _flightCoreServicesMock
                .Setup(service => service.GetAllFlights(It.IsAny<PaginationArgs>()))
                .Returns((PaginationArgs args) =>
                {
                    var data = new FlightListDto
                    {
                        Total = _flightsMockData.Items.Count(),
                        Items = _flightsMockData.Items
                        .Skip(args.StartIndex.Value)
                        .Take(args.PageSize.Value)
                    };

                    return Task.FromResult(data);
                });

            _flightCoreServicesMock
                .Setup(service => service.GetFlight(It.IsAny<GetFlightArgs>()))
                .Returns(async (GetFlightArgs args) => await Task.FromResult(_flightsMockData.Items.Where(f => f.Id == args.FlightId).FirstOrDefault()));

            _flightCoreServicesMock
                .Setup(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()))
                .Returns(Task.FromResult(new FlightDto()));

            _flightCoreServicesMock
                .Setup(service => service.UpdateFlight(It.IsAny<UpdateFlightArgs>()))
                .Returns(Task.FromResult(new FlightDto()));

            _flightCoreServicesMock
                .Setup(service => service.DeleteFlight(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            _airportCoreServicesMock = new Mock<IAirportServices>();
            _airportCoreServicesMock
                .Setup(service => service.GetAllAirports(It.IsAny<PaginationArgs>()))
                .Returns(Task.FromResult(_airportsMockData.AsEnumerable()));

            _aircraftCoreServicesMock = new Mock<IAircraftServices>();
            _aircraftCoreServicesMock
                .Setup(service => service.GetAllAircrafts(It.IsAny<PaginationArgs>()))
                .Returns(Task.FromResult(_aircraftsMockData.AsEnumerable()));
        }

        #region Get Flights
        [TestMethod]
        public async Task FlightControllerTests_Index_ShouldReturnAllFlights()
        {
            // Arrange
            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var currentFlights = await flightController.Index(int.MaxValue, 0);
            var viewResult = currentFlights as ViewResult;
            var flightsData = viewResult.Model as PaginatedFlightList;

            // Assert
            Check.That(currentFlights).IsNotNull();
            Check.That(viewResult.Model).IsNotNull();
            Check.That(flightsData.Items).IsNotNull();
            Check.That(flightsData.TotalCount).IsEqualTo(_flightsMockData.Items.Count());
            Check.That(flightsData.TotalPages).IsEqualTo((int)Math.Ceiling(flightsData.TotalCount / (double)flightsData.PageSize));
            Check.That(flightsData.Items.ToList()).ContainsExactly(_flightsMockData.Items);
            _flightCoreServicesMock.Verify(service => service.GetAllFlights(It.IsAny<PaginationArgs>()), Times.Once);

        }

        [TestMethod]
        public async Task FlightControllerTests_Index_ShouldPaginateFlights()
        {
            // Arrange
            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var currentFlights = await flightController.Index(5, 0);
            var viewResult = currentFlights as ViewResult;
            var flightsData = viewResult.Model as PaginatedFlightList;

            // Assert
            Check.That(currentFlights).IsNotNull();
            Check.That(viewResult.Model).IsNotNull();
            Check.That(flightsData.Items).IsNotNull();
            Check.That(flightsData.Items.Count()).IsEqualTo(5);
            Check.That(flightsData.TotalPages).IsEqualTo((int)Math.Ceiling(flightsData.TotalCount / (double)flightsData.PageSize));

            Check.That(flightsData.TotalCount).IsEqualTo(_flightsMockData.Items.Count());
            _flightCoreServicesMock.Verify(service => service.GetAllFlights(It.IsAny<PaginationArgs>()), Times.Once);
        }

        #endregion

        #region Create New Flight

        [TestMethod]
        public async Task FlightControllerTests_Create_ShouldInitializeViewWithReferenceList()
        {
            // Arrange
            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var viewResult = await flightController.CreateFlight() as ViewResult;
            var viewData = viewResult.ViewData as ViewDataDictionary;

            // Expected
            var expectedViewDataKeys = new List<string> { "aircrafts", "airports" };
            var expectedViewDataValues = new List<object> { _aircraftsMockData, _airportsMockData };
            // Assert
            Check.That(viewResult).IsNotNull();
            Check.That(viewData).IsNotNull();
            Check.That(viewData.Keys.ToList()).ContainsExactly(expectedViewDataKeys);
            Check.That(viewData.Values.ToList()).ContainsExactly(expectedViewDataValues);
        }

        [TestMethod]
        public async Task FlightControllerTests_Create_ShouldReturnBadRequest_WhenFlightNumberIsEmpty()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 1,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("FlightNumber", "Flight number is mandatory");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Create_ShouldReturnBadRequest_WhenDepartureIdIsZero()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 1,
                DepartureAirportId = 0,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("DepartureAirportId", "Departure Airport Id is not Valid.");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Create_ShouldReturnBadRequest_WhenDestinationIdIsZero()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 1,
                DepartureAirportId = 1,
                DestinationAirportId = 0,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("DestinationAirportId", "Destionation Airport Id is not Valid.");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Create_ShouldReturnBadRequest_WhenAircraftIdIsZero()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 0,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("AircraftId", "Aircraft Id is not Valid.");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Create_ShouldCallFlightService_WhenModelIsValid()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "NewFlight",
                AircraftId = 1,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(RedirectToActionResult));
            var viewResult = result as RedirectToActionResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsTrue();
            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Once);
        }
        #endregion

        #region Update A Flight

        [TestMethod]
        public async Task FlightControllerTests_Update_ShouldInitializeViewWithReferenceList()
        {
            // Arrange
            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var viewResult = await flightController.CreateFlight() as ViewResult;
            var viewData = viewResult.ViewData as ViewDataDictionary;

            // Expected
            var expectedViewDataKeys = new List<string> { "aircrafts", "airports" };
            var expectedViewDataValues = new List<object> { _aircraftsMockData, _airportsMockData };
            // Assert
            Check.That(viewResult).IsNotNull();
            Check.That(viewData).IsNotNull();
            Check.That(viewData.Keys.ToList()).ContainsExactly(expectedViewDataKeys);
            Check.That(viewData.Values.ToList()).ContainsExactly(expectedViewDataValues);
        }

        [TestMethod]
        public async Task FlightControllerTests_Update_ShouldReturnBadRequest_WhenFlightNumberIsEmpty()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 1,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("FlightNumber", "Flight number is mandatory");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Update_ShouldReturnBadRequest_WhenDepartureIdIsZero()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 1,
                DepartureAirportId = 0,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("DepartureAirportId", "Departure Airport Id is not Valid.");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Update_ShouldReturnBadRequest_WhenDestinationIdIsZero()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 1,
                DepartureAirportId = 1,
                DestinationAirportId = 0,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("DestinationAirportId", "Destionation Airport Id is not Valid.");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Update_ShouldReturnBadRequest_WhenAircraftIdIsZero()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = string.Empty,
                AircraftId = 0,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);
            flightController.ModelState.AddModelError("AircraftId", "Aircraft Id is not Valid.");

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(BadRequestObjectResult));
            var viewResult = result as BadRequestObjectResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsFalse();
            Check.That(viewResult.StatusCode).IsEqualTo(400);

            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Never);
        }

        [TestMethod]
        public async Task FlightControllerTests_Update_ShouldCallFlightService_WhenModelIsValid()
        {
            // Arrange
            var newFlight = new CreateFlightArgs
            {
                FlightNumber = "NewFlight",
                AircraftId = 1,
                DepartureAirportId = 1,
                DestinationAirportId = 2,
                FlightDate = DateTime.Now,
                FlightTime = DateTime.Now
            };

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var result = await flightController.CreateFlightPost(newFlight);

            // Assert
            Check.That(result).IsInstanceOfType(typeof(RedirectToActionResult));
            var viewResult = result as RedirectToActionResult;

            Check.That(viewResult).IsNotNull();
            Check.That(flightController.ModelState.IsValid).IsTrue();
            _flightCoreServicesMock.Verify(service => service.CreateFlight(It.IsAny<CreateFlightArgs>()), Times.Once);
        }

        #endregion

        #region Delete Flight

        [TestMethod]
        public void FlightControllerTests_Delete_ShouldThrowFlightNoFoundExceptionWhenFlightIdIsNotFound()
        {
            // Arrange
            var flightId = 97973;
            _flightCoreServicesMock.Setup(service => service.DeleteFlight(It.IsAny<int>()))
                .Throws(new FlightNotFoundException(404, string.Format(ErrorMessages.FLIGHT_NOT_FOUND, flightId)));

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            Check.ThatCode(async () => await flightController.ConfirmDeleteFlight(flightId))
                .Throws<FlightNotFoundException>()
                .WithMessage(string.Format(ErrorMessages.FLIGHT_NOT_FOUND, flightId));
        }

        [TestMethod]
        public async Task FlightControllerTests_Delete_ShouldCallFlightService_Successfuly()
        {
            // Arrange
            var flightId = 1;

            var flightController = new FlightsController(_flightCoreServicesMock.Object, _airportCoreServicesMock.Object, _aircraftCoreServicesMock.Object);

            // Act
            var result = await flightController.ConfirmDeleteFlight(flightId);

            // Assert
            _flightCoreServicesMock.Verify(service => service.DeleteFlight(It.IsAny<int>()), Times.Once);
        }

        #endregion

        private void InitMockData()
        {
            _flightsMockData = new FlightListDto();
            var random = new Random();
            var flightItems = new List<FlightDto>();

            for (int i = 1; i < 10; i++)
            {
                flightItems.Add(new FlightDto
                {
                    Id = i,
                    FlightNumber = "FakeFlight" + i,
                    AircraftId = i,
                    DepartureAirportId = i,
                    DestinationAirportId = i + 1,
                    FlightDate = DateTime.Now,
                    FlightTime = DateTime.Now
                });
            }

            _flightsMockData.Items = flightItems;
            _flightsMockData.Total = flightItems.Count;

            _airportsMockData = new List<AirportDto>();
            for (int i = 1; i < 20; i++)
            {
                _airportsMockData.Add(new AirportDto
                {
                    Id = i,
                    Value = "FakeAirport_" + i
                });
            }

            _aircraftsMockData = new List<AircraftDto>();
            for (int i = 1; i < 20; i++)
            {
                _aircraftsMockData.Add(new AircraftDto
                {
                    Id = i,
                    Value = "FakeAircraft_" + i
                });
            }
        }
    }
}
