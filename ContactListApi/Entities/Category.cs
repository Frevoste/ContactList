using System.ComponentModel.DataAnnotations;

namespace ContactListApi.Entities
{
    /*
     * Tutaj rozważałem użycie ENUM aczkolwiek implementacja dowolnej podkategorii dla pracowników oznaczonych OTHER była dla mnie problematyczna.
     */
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

    }
}
