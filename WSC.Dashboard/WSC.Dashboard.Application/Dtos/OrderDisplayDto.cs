using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.Dashboard.Application.Dtos
{
    public class OrderDisplayDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public List<OrderItemsDisplayDto> OrderItems { get; set; }
        public DeliveryDisplayDto Delivery { get; set; }
    }
}
