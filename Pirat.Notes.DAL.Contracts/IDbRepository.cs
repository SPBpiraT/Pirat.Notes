using System.Collections.Generic;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Contracts
{
    public interface IDbRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(int id);
        List<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}