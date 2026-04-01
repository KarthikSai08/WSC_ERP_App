using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Domain.Entities
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }
        public string OpportunityName { get; set; } = null!;
        public OpportunityStage Stage { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
