using System.ComponentModel.DataAnnotations;

namespace WSC.Store.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }  
    }
}
