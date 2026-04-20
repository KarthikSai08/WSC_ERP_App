namespace WSC.Store.Application.Dtos
{
    public sealed record CreateItemsDto(int OrderId, int ProductId, int Quantity, decimal UnitPrice);
}
