﻿using System.Collections.Generic;
using System.Linq;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Implementations
{
    public abstract class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DataContext _context;

        protected DbRepository(DataContext context)
        {
            _context = context;
        }

        public TEntity GetById(int id)
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(x => x.Id == id);

            if (entity == null) throw new KeyNotFoundException("Entity not found.");

            return entity;
        }

        public List<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }
    }
}