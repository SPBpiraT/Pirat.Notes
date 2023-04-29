using System;

namespace Pirat.Notes.Domain.Contracts.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime RegisterDate { get; set; }
        public string UserRole { get; set; }
    }
}