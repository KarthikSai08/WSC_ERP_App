
using System.ComponentModel.DataAnnotations;

namespace WSC.Store.Domain.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int InStock { get; set; }
        public int MinStock { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
