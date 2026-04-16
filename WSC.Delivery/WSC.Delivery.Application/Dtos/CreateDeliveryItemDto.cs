namespace WSC.Delivery.Application.Dtos
{
    public class CreateDeliveryItemDto
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
