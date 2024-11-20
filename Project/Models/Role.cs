using Project.Types;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Roles RoleName { get; set; }
        public bool Status { get; set; }
    }
}
