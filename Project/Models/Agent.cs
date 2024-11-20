using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Agent
    {
        [Key]
        public Guid AgentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public int MobileNumber { get; set; }
        public User User { get; set; }
        public List<Customer> Customers { get; set; }
        public double CommisionEarned { get; set; }
        public bool Satus { get; set; }

    }
}
