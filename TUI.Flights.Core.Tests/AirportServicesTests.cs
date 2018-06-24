using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Airport;
using TUI.Flights.Common.Entities;
using TUI.Flights.Core.Mappers;
using TUI.Flights.Core.Services.AirportServices;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Core.Tests
{
    [TestClass]
    public class AirportServicesTests
    {
        private Mock<IRepository<Airport>> _airportRepositoryMock;
        private IMapper _mapper;

        private List<Airport> _airportsRecordsMock;

        [TestInitialize]
        public void Init()
        {
            var automapperConfig = new MapperConfiguration(acfg =>
            {
                acfg.AddProfile(new AutoMapperConfigurations());
                acfg.ForAllMaps((map, exp) => exp.ForAllOtherMembers(opt => opt.AllowNull()));
            });

            _mapper = automapperConfig.CreateMapper();

            InitializeAndSetupMocks();

        }

        #region Get All Airports
        [TestMethod]
        public async Task AirportServicesTests_GetAllAirportss_ShouldReturnAllAirports()
        {
            // Arrange
            var airportServices = new AirportServices(_airportRepositoryMock.Object, _mapper);

            // Expected
            var expectedAirports = _mapper.Map<IEnumerable<AirportDto>>(_airportsRecordsMock);

            // Act
            var currentAirports = await airportServices.GetAllAirports(new PaginationArgs
            {
                PageSize = int.MaxValue,
                StartIndex = 0
            });

            // Assert
            Check.That(currentAirports).IsNotNull();
            Check.That(currentAirports.Count()).IsEqualTo(expectedAirports.Count());
            Check.That(currentAirports.ToList()).HasFieldsWithSameValues(expectedAirports.ToList());
            _airportRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        #endregion

        private void InitializeAndSetupMocks()
        {
            var random = new Random();

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

            _airportRepositoryMock = new Mock<IRepository<Airport>>();

            #region Airport Repository Mock Setup
            _airportRepositoryMock.Setup(repo => repo.GetAllAsync())
                .Returns(async () => await _airportsRecordsMock.ToAsyncEnumerable().ToList());
            #endregion
        }
    }
}
