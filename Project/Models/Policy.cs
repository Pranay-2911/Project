using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Policy
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
