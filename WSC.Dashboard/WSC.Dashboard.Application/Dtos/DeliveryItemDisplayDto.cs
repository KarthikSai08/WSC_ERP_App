using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Dashboard.Application.Dtos
{
    public sealed record DeliveryItemDisplayDto
    {
        public int DeliveryItemId { get; set; }
        public int Quantity { get; set; }
        public ProductDisplayDto Product { get; set; } = null!;
    }
}
