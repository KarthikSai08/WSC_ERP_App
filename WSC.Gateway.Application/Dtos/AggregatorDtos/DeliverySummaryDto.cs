using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Gateway.Application.Dtos.AggregatorDtos
{
    public record DeliverySummaryDto
    {
        public int TotalDeliveries { get; set; }
        public int ActiveAgents { get; set; }
        public IEnumerable<OrderDeliveryResponseDto> RecentDeliveries { get; set; }
    }
}
