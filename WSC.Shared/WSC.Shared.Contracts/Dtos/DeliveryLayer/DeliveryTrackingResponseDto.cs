

using WSC.Delivery.Domain.Enums;

namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class DeliveryTrackingResponseDto
    {
        public int TrackingId { get; set; }
        public int DeliveryId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
