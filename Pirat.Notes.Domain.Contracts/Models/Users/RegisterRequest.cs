using System.ComponentModel.DataAnnotations;

namespace Pirat.Notes.Domain.Contracts.Models.Users
{
    public class RegisterRequest
    {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}