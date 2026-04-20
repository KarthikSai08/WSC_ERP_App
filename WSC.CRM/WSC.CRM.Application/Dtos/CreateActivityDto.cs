using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public sealed record CreateActivityDto(
        string Title,
        string? Description,
        ActivityType Type,
        DateTime ScheduledAt,
        int? LeadId,
        int? OpportunityId,
        int? CustomerId);
}
