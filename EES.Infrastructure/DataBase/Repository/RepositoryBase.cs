﻿using EES.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EES.Infrastructure.DataBase.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected DbContext Context { get; }

        /// <summary>
        /// DbSet
        /// </summary>
        protected DbSet<T> DbSet { get; }

        public RepositoryBase(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            DbSet.AddRange(entity);
        }

        public async Task AddRangeAsync(params T[] entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }


        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public IQueryable<T> Query()
        {
            return DbSet;
        }

        public IQueryable<T> NoTrackingQuery()
        {
            return DbSet.AsNoTracking();
        }


        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(params T[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(params T[] entities)
        {
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 手动开启事务
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return Context.Database.CurrentTransaction ?? await Context.Database.BeginTransactionAsync(isolation);
        }
    }
}
