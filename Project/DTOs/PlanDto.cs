using System.ComponentModel.DataAnnotations;
namespace Project.DTOs
{
    public class PlanDto
    {
        [Required]
        public string Name { get; set; }
    }
}
