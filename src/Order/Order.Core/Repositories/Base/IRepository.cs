﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Order.Core.Entities.Base;

namespace Order.Core.Repositories.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null
            , Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            , bool disableTracking = true
            , string includeString = null);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null
            , Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            , bool disableTracking = true
            , List<Expression<Func<T, object>>> includes = null);


        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}