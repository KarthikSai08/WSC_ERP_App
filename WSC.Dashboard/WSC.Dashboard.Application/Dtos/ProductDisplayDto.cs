using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WSC.Dashboard.Application.Dtos
{
    public sealed record ProductDisplayDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; }
        public decimal Price { get; set; }
    }
}
