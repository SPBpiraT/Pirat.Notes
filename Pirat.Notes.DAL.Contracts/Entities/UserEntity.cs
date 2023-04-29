using System;
using System.Text.Json.Serialization;
using Pirat.Notes.DAL.Contracts.Roles;

namespace Pirat.Notes.DAL.Contracts.Entities
{
    public class UserEntity : IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime RegisterDate { get; set; }
        public Role UserRole { get; set; }

        [JsonIgnore] public string PasswordHash { get; set; }

        public int Id { get; set; }
    }
}