using System.Collections.Generic;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Users;

namespace Pirat.Notes.Domain.Contracts.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        List<UserModel> GetAll();
        UserModel GetById(int id);
        void Register(RegisterRequest model);
        void Update(int id, UpdateRequest model);
        void Delete(int id);
    }
}