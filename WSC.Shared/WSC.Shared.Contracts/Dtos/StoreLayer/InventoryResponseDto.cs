namespace WSC.Shared.Contracts.Dtos.StoreLayer
{
    public class InventoryResponseDto
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int InStock { get; set; }
    }
}
