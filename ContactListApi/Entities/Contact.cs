
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace ContactListApi.Entities
{
    //Wymuszenie unikatowości adresu email 
    [Index(nameof(Email), IsUnique = true)]
    public class Contact
    {
        //Primary key
        [Key]
        public int Id { get; set; }
        //Fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        //Foreign Key to Category
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int? SubCategoryId { get; set; }
        public virtual SubCategory? SubCategory { get; set; }

    }
}
