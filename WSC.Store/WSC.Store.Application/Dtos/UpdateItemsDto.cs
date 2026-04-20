namespace WSC.Store.Application.Dtos
{
    public sealed record UpdateItemsDto(int OrderItemId, int ProductId, int Quantity, decimal UnitPrice);
}
