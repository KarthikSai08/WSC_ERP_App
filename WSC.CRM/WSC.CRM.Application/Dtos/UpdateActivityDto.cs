using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public sealed class UpdateActivityDto
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ActivityType Type { get; set; }
        public int? LeadId { get; set; }
        public int? OpportunityId { get; set; }
        public int? CustomerId { get; set; }

    }
}
