namespace WSC.Shared.Contracts.Dtos.StoreLayer
{
    public sealed class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }  // integrates with CRM
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
