using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public class CreateActivityDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public ActivityType Type { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int LeadId { get; set; }
    }
}
