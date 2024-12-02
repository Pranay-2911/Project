﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Types;

namespace Project.Models
{
    public class Policy
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength =5, ErrorMessage ="Title must be within the 5 to 100 characters")]
        public string Title { get; set; }
        [Required]
        [StringLength(500, MinimumLength =5, ErrorMessage = "Description must be within the 5 to 100 characters")]
        public string Description { get; set; }
        [Required]
        public double MinAmount { get; set; }
        [Required]
        public double MaxAmount { get; set; }
        [Required]
        public int MinAge { get; set; }
        [Required]
        public int MaxAge { get; set; }
        [Required]
        public int MinPolicyTerm { get; set; }
        [Required]
        public int MaxPolicyTerm { get; set; }
        [Required]
        public int policyRatio { get; set; }
        [Required]
        public bool PolicyStatus { get; set; }
        [Required]
        public double RegistrationCommisionAmount { get; set; }
        [Required]
        public int InstallmentCommisionRatio { get; set; }
        public DocumentType DocumentType { get; set; }  
        
        public List<PolicyAccount> Accounts { get; set; }
        public Plan Plan { get; set; }
        [ForeignKey("Plan")]
        public Guid PlanId { get; set; }

    }
}
