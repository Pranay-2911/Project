using System.ComponentModel.DataAnnotations;
namespace Project.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public Guid Id { get; set; }    
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
