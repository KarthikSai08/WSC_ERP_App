using WSC.Shared.Contracts.Enums;

namespace WSC.Shared.Contracts.Dtos.CRMLayer
{
    public sealed class LeadResponseDto
    {
        public int LeadId { get; set; }
        public string LeadName { get; set; }
        public string LeadPhone { get; set; }

        public LeadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

    }
}
