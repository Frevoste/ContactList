using ContactListApi.Migrations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ContactListApi.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class CreateContactDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Category { get; set; } = "Other";
        public string? SubCategory { get; set; } = "New";
    }
}
