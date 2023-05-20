using System.Linq;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Implementations
{
    public class UserRepository : DbRepository<UserEntity>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        UserEntity IUserRepository.GetByUsername(UserEntity entity)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == entity.Username);

            return user;
        }

        bool IUserRepository.IsUsernameExist(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }
    }
}