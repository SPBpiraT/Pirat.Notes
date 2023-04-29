using System.Linq;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Implementations
{
    public class UserRepository : DbRepository, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        UserEntity IUserRepository.GetByUsername(UserEntity entity)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == entity.Username);

            return user;
        }

        void IUserRepository.Add(UserEntity newEntity)
        {
            _context.Users.Add(newEntity);

            _context.SaveChanges();
        }

        void IUserRepository.Delete(int entityId)
        {
            var activeEntity = _context.Users.FirstOrDefault(x => x.Id == entityId);

            _context.Users.Remove(activeEntity);

            _context.SaveChanges();
        }

        void IUserRepository.Update(UserEntity entity)
        {
            _context.Users.Update(entity);

            _context.SaveChanges();
        }

        bool IUserRepository.IsUsernameExist(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }
    }
}