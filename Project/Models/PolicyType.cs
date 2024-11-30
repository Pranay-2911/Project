using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PolicyType
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Policy> Policies { get; set; }
        
    }
}