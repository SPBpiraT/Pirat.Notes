using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Contracts
{
    public interface INoteRepository : IDbRepository<NoteEntity>
    {
    }
}