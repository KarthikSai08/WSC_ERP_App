namespace WSC.Delivery.Application.Dtos
{
    public class UpdateDeliveryItemDto
    {
        public int DeliveryItemId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool IsDelivered { get; set; }
    }
}
