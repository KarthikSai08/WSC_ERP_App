using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public sealed record CreateOpportunityDto(string OpportunityName, OpportunityStage Stage, decimal Amount,
                                    DateTime? ClosedAt, int? CustomerId, int LeadId);
}
