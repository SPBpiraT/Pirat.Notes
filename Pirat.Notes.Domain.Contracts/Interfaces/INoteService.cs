using System.Collections.Generic;
using Pirat.Notes.Domain.Contracts.Models;

namespace Pirat.Notes.Domain.Contracts.Interfaces
{
    public interface INoteService
    {
        NoteModel GetById(int id);
        List<NoteModel> GetAll();
        void CreateNote(NoteModel note);
        void Delete(int id);
    }
}