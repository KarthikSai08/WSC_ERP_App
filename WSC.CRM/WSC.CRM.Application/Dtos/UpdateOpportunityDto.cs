using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public sealed record UpdateOpportunityDto(int OpportunityId, string? OpportunityName, OpportunityStage? Stage,
                                    decimal? Amount, int? CustomerId);
}
