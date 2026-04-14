namespace WSC.Delivery.Domain.Entities
{
    public class DeliveryItem
    {
        public int DeliveryItemId { get; set; }
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }   // From Store module
        public int Quantity { get; set; }
    }
}
