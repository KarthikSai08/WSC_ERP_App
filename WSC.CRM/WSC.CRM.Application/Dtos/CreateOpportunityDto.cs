using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public class CreateOpportunityDto
    {
        [Required]
        public string OpportunityName { get; set; } = null!;
        [Required]
        public OpportunityStage Stage { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ClosedAt { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}
