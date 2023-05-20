using System.Linq;
using Pirat.Notes.DAL.Contracts;
using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.DAL.Implementations
{
    public class NoteRepository : DbRepository<NoteEntity>, INoteRepository
    {
        public NoteRepository(DataContext context) : base(context)
        {
        }
    }
}