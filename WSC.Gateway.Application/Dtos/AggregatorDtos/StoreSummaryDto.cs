using WSC.Shared.Contracts.Dtos.StoreLayer;

namespace WSC.Gateway.Application.Dtos.AggregatorDtos
{
    public record StoreSummaryDto
    {
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public IEnumerable<OrderResponseDto> RecentOrders { get; set; }
    }
}
