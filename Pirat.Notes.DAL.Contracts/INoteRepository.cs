using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Contracts
{
    public interface INoteRepository : IDbRepository<NoteEntity>
    {
        void AddNote(NoteEntity newEntity);
        void DeleteNote(int entityId);
        void UpdateNote(NoteEntity note);
    }
}