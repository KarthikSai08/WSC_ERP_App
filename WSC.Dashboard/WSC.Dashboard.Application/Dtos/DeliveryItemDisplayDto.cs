using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Dashboard.Application.Dtos
{
    public sealed class DeliveryItemDisplayDto
    {
        public int DeliveryItemId { get; set; }
        public int Quantity { get; set; }
        public ProductDisplayDto Product { get; set; } = null!;
    }
}
