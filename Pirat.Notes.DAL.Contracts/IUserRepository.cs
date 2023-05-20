using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Contracts
{
    public interface IUserRepository : IDbRepository<UserEntity>
    {
        UserEntity GetByUsername(UserEntity entity);
        bool IsUsernameExist(string username);
    }
}