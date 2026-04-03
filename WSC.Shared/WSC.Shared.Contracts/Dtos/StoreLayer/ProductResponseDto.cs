using System.ComponentModel.DataAnnotations;

namespace WSC.Shared.Contracts.Dtos.StoreLayer
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
