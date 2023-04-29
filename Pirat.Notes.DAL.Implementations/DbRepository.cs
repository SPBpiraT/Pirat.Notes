using System.Collections.Generic;
using System.Linq;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Implementations
{
    public abstract class DbRepository : IDbRepository<IEntity>
    {
        protected readonly DataContext _context;

        public DbRepository(DataContext context)
        {
            _context = context;
        }

        public T GetById<T>(int id) where T : class, IEntity
        {
            var entity = _context.Set<T>().FirstOrDefault(x => x.Id == id);

            if (entity == null) throw new KeyNotFoundException("Entity not found.");

            return entity;
        }

        public List<T> GetAll<T>() where T : class, IEntity
        {
            return _context.Set<T>().ToList<T>();
        }
    }
}