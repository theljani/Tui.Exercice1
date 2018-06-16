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
        T Get(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(object id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        IQueryable<T> GetAll(int pageSize, int startIndex);
        Task<IEnumerable<T>> GetAllAsync(int pageSize, int startIndex);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> filter, int pageSize, int startIndex);

        T Add(T item);
        Task<T> AddAsync(T item);

        T Update(T item);
        Task<T> UpdateAsync(T item);

        void Delete(object id);
        Task<T> DeleteAsync(object id);

        EntityEntry GetEntry(T entity);
    }
}
