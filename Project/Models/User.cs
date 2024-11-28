using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } 
        [Required]
        [StringLength(20, MinimumLength=5, ErrorMessage = "username must be in 5 to 20 characters")]   
        public string UserName { get; set; }
        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }

        public bool Status { get; set; }
        public Role Role { get; set; }  
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }


    }
}
