﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Entities.Base;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get and Get Async
        public T Get(object id)
        {
            return GetSet().Find(id);
        }

        public async Task<T> GetAsync(object id)
        {
            return await GetSet().FindAsync(id);
        }
        #endregion

        #region GetAll and GetAllAsync and Search

        public IQueryable<T> GetAll()
        {
            return GetSet().AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetSet().ToListAsync();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> filter)
        {
            return GetSet().Where(filter).AsQueryable();
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> filter)
        {
            return await GetSet().Where(filter).AsQueryable().ToListAsync();
        }

        #endregion

        #region Add and Add Async
        public T Add(T item)
        {
            var entity = GetSet().Add(item).Entity;
            _unitOfWork.Commit();
            return entity;
        }

        public async Task<T> AddAsync(T item)
        {
            var data = GetSet().Add(item).Entity;
            await _unitOfWork.CommitAsync();
            return data;
        }
        #endregion

        #region Update and Update Async
        public T Update(T item)
        {
            var entity = GetSet().Update(item).Entity;
            _unitOfWork.Commit();
            return entity;
        }

        public async Task<T> UpdateAsync(T item)
        {
            var entity = GetSet().Update(item).Entity;
            await _unitOfWork.CommitAsync();
            return entity;
        }
        #endregion

        #region Delete And Delete Async
        public void Delete(object id)
        {
            var entity = Get(id);

            if (entity != null)
            {
                GetSet().Remove(entity);
                _unitOfWork.Commit();
            }
        }

        public async Task<T> DeleteAsync(object id)
        {
            var item = Get(id);
            var entity = GetSet().Remove(item).Entity;
            await _unitOfWork.CommitAsync();
            return entity;
        }
        #endregion

        public EntityEntry GetEntry(T entity)
        {
            return _unitOfWork.GetEntry<T>(entity);
        }

        private DbSet<T> GetSet()
        {
            return _unitOfWork.CreateSet<T>();
        }

        public int GetTotal()
        {
            return GetSet().Count();
        }
    }
}