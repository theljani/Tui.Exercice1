using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Infrastructure.Base;
using System.Linq;
using TUI.Flights.Infrastructure.Helpers;
using TUI.Flights.Common.Entities;
using TUI.Flights.Common.ValueObjects;

namespace TUI.Flights.Infrastructure.Data
{
    public static class DbSeeder
    {
        #region Public Methods
        public static void Seed(EFUnitOfWork dbContext)
        {
            if (!dbContext.Airports.Any())
            {
                CreateAirports(dbContext);
            }

            if (!dbContext.Aircrafts.Any())
            {
                CreateAircrafts(dbContext);
            }
        }
        #endregion

        #region Seed Methods
        private static void CreateAirports(IUnitOfWork dbContext)
        {
            string airportsJsonUrl = "https://gist.githubusercontent.com/tdreyno/4278655/raw/7b0762c09b519f40397e4c3e100b097d861f5588/airports.json";
            var airports = JsonMapper.ReadFromJson<AirportSeedModel>(airportsJsonUrl);

            airports.ToList().ForEach(data =>
            {
                var newAirport = new Airport
                {
                    Code = data.Code,
                    Name = data.Name,
                    Latitude = double.Parse(data.Latitude.Replace(".", ",")),
                    Longitude = double.Parse(data.Longitude.Replace(".", ",")),
                    Email = data.Email,
                    Phone = data.Phone,
                    Website = data.Website,
                    //Location = new Location(data.Country, data.City, data.State, data.TimeZone),
                    CreationDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow
                };

                dbContext.Airports.Add(newAirport);
            });

            dbContext.Commit();
        }

        private static void CreateAircrafts(IUnitOfWork dbContext)
        {
            var aircrafts = new List<Aircraft>
            {
                new Aircraft
                {
                    Code= "A300",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A300",
                    ConsumptionKgPerH = 4770,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 25,
                    TakeOffEffort = 35,
                    ConstructionYear = DateTime.UtcNow,
                    MilesPerHour = 144,
                    Company = "Airbus",
                    Description = "medium-range widebody airliner"
                },
                new Aircraft
                {
                    Code= "A310",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A310",
                    ConsumptionKgPerH = 4500 ,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 23,
                    MilesPerHour = 144,
                    TakeOffEffort = 35,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "widebody airliner"
                },
                new Aircraft
                {
                    Code= "A318",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A318",
                    ConsumptionKgPerH = 2200,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 18,
                    TakeOffEffort = 35,
                    MilesPerHour = 144,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "airliner and large corporate jet"
                },
                new Aircraft
                {
                    Code= "A319",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A319",
                    ConsumptionKgPerH = 2370,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 25,
                    TakeOffEffort = 35,
                    MilesPerHour = 144,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "airliner and large corporate jet"
                },
                new Aircraft
                {
                    Code= "A320",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A320",
                    ConsumptionKgPerH = 2500,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 18,
                    TakeOffEffort = 35,
                    MilesPerHour = 144,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "airliner"
                },
                new Aircraft
                {
                    Code= "A321",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A321",
                    ConsumptionKgPerH = 2900,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 22,
                    TakeOffEffort = 35,
                    MilesPerHour = 144,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "narrowbody airliner"
                },
                new Aircraft
                {
                    Code= "A330",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus A330",
                    ConsumptionKgPerH = 5600,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 21,
                    TakeOffEffort = 35,
                    MilesPerHour = 144,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "widebody airliner"
                },
                new Aircraft
                {
                    Code= "Beluga",
                    SerialNumber = Guid.NewGuid().ToString(),
                    Model = "Airbus Beluga",
                    ConsumptionKgPerH = 5200,
                    Capacity = 250,
                    Height = 6,
                    Length = 30,
                    Weight = 30,
                    TakeOffEffort = 35,
                    MilesPerHour = 144,
                    ConstructionYear = DateTime.UtcNow,
                    Company = "Airbus",
                    Description = "high-capacity transport aircraft"
                }
            };

            aircrafts.ForEach(aircraft =>
            {
                dbContext.Aircrafts.Add(aircraft);
            });

            dbContext.Commit();
        }
        #endregion
    }
}
