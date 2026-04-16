using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Interfaces.ServiceInterfaces
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetAllProductsAsync(CancellationToken ct);
        Task<ApiResponse<ProductResponseDto?>> GetProductByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<int>> CreateProductAsync(CreateProductDto prd, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateProductAsync(UpdateProductDto prd, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteProductAsync(int id, CancellationToken ct);
    }
}
