using System.ComponentModel.DataAnnotations;
using WSC.Store.Domain.Enums;

namespace WSC.Store.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }  // integrates with CRM
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
