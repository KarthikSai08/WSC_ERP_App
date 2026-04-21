namespace WSC.Gateway.Application.Dtos.AggregatorDtos
{
    public record DeliverySummaryDto
    {
        public int TotalDeliveries { get; set; }
        public int ActiveAgents { get; set; }
        public IEnumerable<DeliveryResponseDto> RecentDeliveries { get; set; }
    }
}
