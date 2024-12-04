using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Admin
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(15, ErrorMessage ="First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Last name should not greater than 15")]
        public string LastName { get; set; }
        
        public User User { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

    }
}
