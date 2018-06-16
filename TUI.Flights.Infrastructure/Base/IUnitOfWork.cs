using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using TUI.Flights.Common.Entities;

namespace TUI.Flights.Infrastructure.Base
{
    public interface IUnitOfWork
    {
        DbSet<Flight> Flights { get; }
        DbSet<Aircraft> Aircrafts { get; }
        DbSet<Airport> Airports { get; }

        void Commit();
        Task<int> CommitAsync();
        void RollBack();

        EntityEntry GetEntry<T>(T entity) where T : class;
        DbSet<T> CreateSet<T>() where T : class;
    }
}
