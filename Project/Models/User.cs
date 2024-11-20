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
        public string userName { get; set; }
        [Required]
        [StringLength(20, MinimumLength=7, ErrorMessage = "password must be in 7 to 20 characters")]    
        public string password { get; set; }
        public bool Status { get; set; }

        public Role Role { get; set; }  
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
    }
}
