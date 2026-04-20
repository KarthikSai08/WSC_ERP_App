using WSC.Shared.Contracts.Enums;

namespace WSC.Shared.Contracts.Dtos.CRMLayer
{
    public sealed record OpportunityResponseDto
    {
        public int OpportunityId { get; set; }
        public string OpportunityName { get; set; } = null!;
        public OpportunityStage Stage { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
