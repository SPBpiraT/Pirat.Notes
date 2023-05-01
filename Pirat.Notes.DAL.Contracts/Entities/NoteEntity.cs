using System;

namespace Pirat.Notes.DAL.Contracts.Entities
{
    public class NoteEntity : IEntity
    {
        public NoteEntity()
        {
            NoteDate = DateTime.Now; // bad idea
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
        public int Id { get; set; }
    }
}