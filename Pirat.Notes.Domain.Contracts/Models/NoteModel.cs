using System;

namespace Pirat.Notes.Domain.Contracts.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
    }
}