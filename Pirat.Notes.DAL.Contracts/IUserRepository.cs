using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Contracts
{
    public interface IUserRepository : IDbRepository<IEntity>
    {
        UserEntity GetByUsername(UserEntity entity);
        void Add(UserEntity newEntity);
        void Delete(int entityId);
        void Update(UserEntity entity);
        bool IsUsernameExist(string username);
    }
}