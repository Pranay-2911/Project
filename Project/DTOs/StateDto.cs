using System.ComponentModel.DataAnnotations;
namespace Project.DTOs
{
    public class StateDto
    {
        [Required]
        public string StateName { get; set; }
        [Required]
        public string CityName { get; set; }
    }
}
