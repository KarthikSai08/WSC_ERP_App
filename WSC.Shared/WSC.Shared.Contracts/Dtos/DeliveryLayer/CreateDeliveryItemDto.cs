namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class CreateDeliveryItemDto
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
