namespace WSC.Store.Application.Dtos
{
    public sealed record UpdateProductDto(int ProductId,
                                string ProductName,
                                string SKU,
                                string? Category,
                                decimal Price);
}
