using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class State
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<City>? Cities { get; set; } = new List<City>();
    }
}
