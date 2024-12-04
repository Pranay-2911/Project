using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PolicyType
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        
        public string Name { get; set; }

        public List<Policy> Policies { get; set; }
        
    }
}