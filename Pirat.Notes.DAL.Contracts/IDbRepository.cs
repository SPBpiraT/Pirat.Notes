using System.Collections.Generic;

namespace Pirat.Notes.DAL.Contracts
{
    public interface IDbRepository<IEntity>
    {
        T GetById<T>(int id) where T : class, IEntity;
        List<T> GetAll<T>() where T : class, IEntity;
    }
}