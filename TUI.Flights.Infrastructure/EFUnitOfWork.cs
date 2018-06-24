using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TUI.Flights.Common.Entities;
using TUI.Flights.Infrastructure.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TUI.Flights.Infrastructure
{
    public class EFUnitOfWork : DbContext, IUnitOfWork
    {
        private DbSet<Flight> _flights;
        private DbSet<Airport> _airports;
        private DbSet<Aircraft> _aircrafts;

        public EFUnitOfWork()
        {

        }

        public EFUnitOfWork(DbContextOptions<EFUnitOfWork> options)
        : base(options)
        {
        }

        public DbSet<Flight> Flights
        {
            get
            {
                if (_flights == null)
                {
                    _flights = base.Set<Flight>();

                }

                return _flights;
            }
        }

        public DbSet<Airport> Airports
        {
            get
            {
                if (_airports == null)
                    _airports = base.Set<Airport>();

                return _airports;
            }
        }

        public DbSet<Aircraft> Aircrafts
        {
            get
            {
                if (_aircrafts == null)
                    _aircrafts = base.Set<Aircraft>();

                return _aircrafts;
            }
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<T> CreateSet<T>() where T : class
        {
            return base.Set<T>();
        }

        // Used to make Explicit loafing instead of lazy loading
        public EntityEntry GetEntry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Flight>(entity =>
            {
                entity
                .HasIndex(e => e.FlightNumber)
                .IsUnique();

                entity
                .HasOne(typeof(Airport), "Departure")
                .WithMany()
                .HasForeignKey("AirportDepartureId")
                .OnDelete(DeleteBehavior.Restrict);

                entity
                .HasOne(typeof(Airport), "Destination")
                .WithMany()
                .HasForeignKey("AirportDestinationId")
                .OnDelete(DeleteBehavior.Restrict);

                entity
                 .HasOne(typeof(Aircraft), "Aircraft")
                 .WithMany()
                 .HasForeignKey("AircraftId")
                 .OnDelete(DeleteBehavior.Restrict);
            });

            //builder.Entity<Airport>(entity =>
            //{
            //    entity.OwnsOne(o => o.Location);
            //});

            builder.Entity<Aircraft>(entity =>
            {
                entity.HasIndex(e => e.Code)
                .IsUnique();
                entity.HasIndex(e => e.SerialNumber)
                .IsUnique();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Activate Lazy loading
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
