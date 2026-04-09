namespace WSC.Delivery.Application.Dtos
{
    public class CreateDeliveryItemDto
    {
        public int DeliveryId { get; set; }
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
