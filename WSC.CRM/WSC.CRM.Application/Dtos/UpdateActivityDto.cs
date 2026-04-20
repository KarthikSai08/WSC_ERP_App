using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public sealed record UpdateActivityDto(int ActivityId, string Title, string Description,
                                            ActivityType Type, int? LeadId, int? OpportunityId,
                                            int? CustomerId);
}
