namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class DeliveryItemResponseDto
    {
        public int DeliveryItemId { get; set; }
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
