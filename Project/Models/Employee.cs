using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Employee
    {

        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MobileNumber { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }
        public User User { get; set; }
        public string status { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public List<Customer> Customers { get; set; }
        public List<Agent> Agents { get; set; }
        public bool Satus { get; set; }

    }
}
