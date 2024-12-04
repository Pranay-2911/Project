using System.ComponentModel.DataAnnotations;
namespace Project.Models
{
    public class City
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage ="City must be in 20 characters")]
        public string Name { get; set; }
        public bool Satus { get; set; }
    }
}
