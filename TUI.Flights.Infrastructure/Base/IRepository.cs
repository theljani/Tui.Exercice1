using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace TUI.Flights.Infrastructure.Base
{
    public interface IRepository<T> where T : class
    {
        T Get(object id);
        Task<T> GetAsync(object id);

        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> Search(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> filter);

        T Add(T item);
        Task<T> AddAsync(T item);

        T Update(T item);
        Task<T> UpdateAsync(T item);

        void Delete(object id);
        Task<T> DeleteAsync(object id);
        int GetTotal();
        EntityEntry GetEntry(T entity);
    }
}
