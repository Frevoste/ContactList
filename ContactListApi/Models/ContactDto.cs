namespace ContactListApi.Models
{
    //DTO Data Tranfer Object - umożliwia ograniczenie danych które chcemy przesłać użytkownikowi api.
    public class ContactDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
    }
}
