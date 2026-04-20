using System.ComponentModel.DataAnnotations;

namespace WSC.Store.Application.Dtos
{
    public sealed class CreateProductDto
    {
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string SKU { get; set; } = null!;
        public string? Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
