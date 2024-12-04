﻿using Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class EmployeeDto
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
        [Required]
        public double Salary { get; set; }
        public Guid UserId { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalAgents { get; set; }
    }
}
