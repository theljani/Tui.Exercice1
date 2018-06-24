using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Aircraft;
using TUI.Flights.Common.Entities;
using TUI.Flights.Core.Mappers;
using TUI.Flights.Core.Services.AircraftServices;
using TUI.Flights.Core.Services.AirportServices;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Core.Tests
{
    [TestClass]
    public class AircraftServicesTests
    {
        private Mock<IRepository<Aircraft>> _aircraftRepositoryMock;
        private IMapper _mapper;

        private List<Aircraft> _aircraftsRecordsMock;

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

        #region Get All Aircrafts
        [TestMethod]
        public async Task AircraftServicesTests_GetAllAircraftss_ShouldReturnAllAircrafts()
        {
            // Arrange
            var aircraftServices = new AircraftServices(_aircraftRepositoryMock.Object, _mapper);

            // Expected
            var expectedAircrafts = _mapper.Map<IEnumerable<AircraftDto>>(_aircraftsRecordsMock);

            // Act
            var currentAircrafts = await aircraftServices.GetAllAircrafts(new PaginationArgs
            {
                PageSize = int.MaxValue,
                StartIndex = 0
            });

            // Assert
            Check.That(currentAircrafts).IsNotNull();
            Check.That(currentAircrafts.Count()).IsEqualTo(expectedAircrafts.Count());
            Check.That(currentAircrafts.ToList()).HasFieldsWithSameValues(expectedAircrafts.ToList());
            _aircraftRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        #endregion

        private void InitializeAndSetupMocks()
        {
            var random = new Random();

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

            _aircraftRepositoryMock = new Mock<IRepository<Aircraft>>();

            #region Aircraft Repository Mock Setup
            _aircraftRepositoryMock.Setup(repo => repo.GetAllAsync())
                .Returns(async () => await _aircraftsRecordsMock.ToAsyncEnumerable().ToList());
            #endregion
        }
    }
}
