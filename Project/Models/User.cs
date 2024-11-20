using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }    
        public string userName { get; set; }    
        public string password { get; set; }
        public Role Role { get; set; }  
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public bool Satus { get; set; }
    }
}
