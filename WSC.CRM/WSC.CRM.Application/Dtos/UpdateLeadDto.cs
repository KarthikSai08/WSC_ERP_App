namespace WSC.CRM.Application.Dtos
{
    public class UpdateLeadDto
    {
        public int LeadId { get; set; }
        public string? LeadName { get; set; }
        public string? LeadEmail { get; set; }
        public string? LeadPhone { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
