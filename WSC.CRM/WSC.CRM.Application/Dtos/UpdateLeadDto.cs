namespace WSC.CRM.Application.Dtos
{
    public sealed record UpdateLeadDto(int LeadId, string? LeadName, string? LeadEmail, string? LeadPhone, DateTime? UpdatedAt);
}
