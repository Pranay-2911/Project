﻿using System.ComponentModel.DataAnnotations;
using Project.Types;

namespace Project.DTOs
{
    public class PurchasePolicyRequestDto
    {
        [Required]
        public Guid PolicyId { get; set; }
        [Required]
        public double TotalAmount { get; set; }
        [Required]
        public int DurationInMonths { get; set; }
        [Required]
        public string Nominee { get; set; }
        [Required]
        public string NomineeRelation { get; set; }
        public Guid? AgentId { get; set; }

    }
}
