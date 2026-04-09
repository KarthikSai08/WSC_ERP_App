using WSC.Store.Domain.Enums;

namespace WSC.Store.Application.Dtos
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }  // integrates with CRM
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
