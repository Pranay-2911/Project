using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Plan
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Policy> Schemes { get; set; }
    }
}
