using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.CRM.Domain.Entities
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }
        public string OpportunityName { get; set; }
        public OpportunityStage Stage { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
