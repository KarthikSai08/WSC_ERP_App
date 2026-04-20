using System.ComponentModel.DataAnnotations;

namespace WSC.Store.Application.Dtos
{
    public sealed class UpdateProductDto
    {
        [Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
    }
}
