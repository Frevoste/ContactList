using System.ComponentModel.DataAnnotations;
namespace ContactListApi.Entities
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
