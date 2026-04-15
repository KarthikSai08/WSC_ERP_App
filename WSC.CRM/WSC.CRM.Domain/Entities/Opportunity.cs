using System.ComponentModel.DataAnnotations;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Domain.Entities
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }
        [Required]
        public string OpportunityName { get; set; } = null!;
        [Required]
        public OpportunityStage Stage { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public int? CustomerId { get; set; } // set after closed won
        public int LeadId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
    }
}
