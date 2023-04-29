using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Notes;
using Pirat.Notes.Domain.Contracts.Models.Users;

namespace Pirat.Notes.Domain.Contracts.Interfaces
{
    public interface IAdminService
    {
        UserModel SetUserRole(int id, UserRoleUpdateRequest userRoleUpdateRequest);
        void DeleteUser(int id);
        void DeleteNote(int id);
        UserModel UpdateUser(int id, UpdateRequest updateRequest);
        NoteModel UpdateNote(int id, NoteUpdateRequest noteUpdateRequest);
    }
}