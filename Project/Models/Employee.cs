﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Employee
    {

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Last name should not greater than 15")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public long MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public double Salary { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public List<Customer> Customers { get; set; }
        public List<Agent> Agents { get; set; }

    }
}
