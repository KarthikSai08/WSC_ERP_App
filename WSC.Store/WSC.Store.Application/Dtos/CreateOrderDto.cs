using WSC.Store.Domain.Enums;

namespace WSC.Store.Application.Dtos
{
    public sealed class CreateOrderDto
    {
        public int CustomerId { get; set; }  // integrates with CRM
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
