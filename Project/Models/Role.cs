using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Satus { get; set; }
    }
}
