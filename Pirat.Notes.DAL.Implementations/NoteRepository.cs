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

        void INoteRepository.AddNote(NoteEntity newNote)
        {
            _context.Notes.Add(newNote);

            _context.SaveChanges();
        }

        void INoteRepository.UpdateNote(NoteEntity entity)
        {
            _context.Notes.Update(entity);

            _context.SaveChanges();
        }

        void INoteRepository.DeleteNote(int noteId)
        {
            var noteEntity = _context.Notes.FirstOrDefault(x => x.Id == noteId);

            _context.Notes.Remove(noteEntity);

            _context.SaveChanges();
        }
    }
}