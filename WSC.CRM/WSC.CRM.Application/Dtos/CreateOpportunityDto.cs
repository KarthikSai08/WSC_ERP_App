using System.ComponentModel.DataAnnotations;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public sealed class CreateOpportunityDto
    {
        public string OpportunityName { get; set; } = null!;
        public OpportunityStage Stage { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int? CustomerId { get; set; }
        public int LeadId { get; set; }
    }
}
