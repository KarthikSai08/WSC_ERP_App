using WSC.Delivery.Domain.Enums;

namespace WSC.Delivery.Application.Dtos
{
    public sealed class CreateDeliveryTrackingDto
    {
        public int DeliveryId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
