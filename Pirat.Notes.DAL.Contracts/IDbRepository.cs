using System.Collections.Generic;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Contracts
{
    public interface IDbRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(int id);
        List<TEntity> GetAll();
        
        //Add other abstracs methods
        //Delete(int id)
        //Update(TEntity)
        //Add(TEntity)
    }
}