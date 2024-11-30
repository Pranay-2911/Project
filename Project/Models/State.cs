using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class State
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<City>? Cities { get; set; } = new List<City>();
    }
}
