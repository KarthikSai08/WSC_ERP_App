using System;
using System.Collections.Generic;
using System.Text;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct);
        Task<Product?> GetProductByIdAsync(int id, CancellationToken ct);
        Task<int> CreateProductAsync(Product prd, CancellationToken ct);
        Task<bool> ExistsBySKUAsync(string email, CancellationToken ct);
        Task<bool> UpdateProductAsync(Product prd, CancellationToken ct);
        Task<bool> DeleteProductAsync(int id, CancellationToken ct);
      //  Task<(IEnumerable<CustomerResponseDto> Data, int TotalCount)> GetPagedCustomersAsync(PaginationRequest request, CancellationToken ct);
    }
}
