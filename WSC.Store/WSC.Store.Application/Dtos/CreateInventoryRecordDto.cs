namespace WSC.Store.Application.Dtos
{
    public sealed record CreateInventoryRecordDto(int ProductId, int InStock, int MinStock);
}
