using Pirat.Notes.DAL.Contracts.Entities;

using System.Collections.Generic;

namespace Pirat.Notes.DAL.Contracts
{
    public interface IDbRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(int id);
        List<TEntity> GetAll();
    }
}