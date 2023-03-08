using System.ComponentModel.DataAnnotations;

namespace ContactListApi.Models
{
    public class UpdateContactDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
    }
}
