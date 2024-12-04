using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class RoleDto
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public int TotalUser { get; set; }
    }
}
