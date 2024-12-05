using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class DocumentDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public Guid CustomerId { get; set; }
    }
}
