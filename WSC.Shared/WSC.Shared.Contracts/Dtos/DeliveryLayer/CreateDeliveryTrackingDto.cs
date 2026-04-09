using WSC.Shared.Contracts.Enums;

namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class CreateDeliveryTrackingDto
    {
        public int DeliveryId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
