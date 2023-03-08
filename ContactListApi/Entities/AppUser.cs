using System.ComponentModel.DataAnnotations;

namespace ContactListApi.Entities
{
    public class AppUser
    {
        [Key]
        public string Email { get; set; }
        public string PasswordHash{ get; set; }
    }
}
