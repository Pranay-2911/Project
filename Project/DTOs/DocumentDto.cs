using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class DocumentDto
    {
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public Guid PolicyAccountId { get; set; }
    }
}
