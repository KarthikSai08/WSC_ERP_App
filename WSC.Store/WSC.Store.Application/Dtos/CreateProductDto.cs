using System.ComponentModel.DataAnnotations;

namespace WSC.Store.Application.Dtos
{
    public sealed record CreateProductDto(string ProductName,
                                         string SKU,
                                         string? Category,
                                         decimal Price);
}
