﻿using System.ComponentModel.DataAnnotations;

namespace Project.DTOs
{
    public class EmployeeRegisterDto
    {
        [Required]
        [StringLength(15, ErrorMessage = "First name should not greater than 15")]
        public string FirstName { get; set; }


        [Required]
        [StringLength(15, ErrorMessage = "Last name should not greater than 15")]
        public string LastName { get; set; }


        [Required]
        [Phone]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public long MobileNumber { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(10000, 100000, ErrorMessage ="Salary must be between 10000 to 100000")]
        public double Salary { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "username must be in 5 to 20 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "password must be in 7 to 20 characters")]
        public string Password { get; set; }
    }
}
