namespace WSC.Gateway.Application.Dtos.AggregatorDtos
{
    public record AppSummaryDto
    {
        public UserProfileDto User { get; set; }
        public CrmSummaryDto Crm { get; set; }
        public StoreSummaryDto Store { get; set; }
        public DeliverySummaryDto Delivery { get; set; }
    }
}
