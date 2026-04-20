namespace WSC.Store.Application.Dtos
{
    public sealed class CreateInventoryRecordDto
    {
        public int ProductId { get; set; }
        public int InStock { get; set; }
        public int MinStock { get; set; }

    }
}
