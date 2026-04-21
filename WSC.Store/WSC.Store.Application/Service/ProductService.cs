using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.Application.Service
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<ApiResponse<int>> CreateProductAsync(CreateProductDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var exists = await _repo.ExistsBySKUAsync(dto.SKU, ct);

            if (exists)
                throw new DuplicateException("Product", dto.SKU);

            var product = _mapper.Map<Domain.Entities.Product>(dto);

            product.SetSKU(dto.SKU);
            var result = await _repo.CreateProductAsync(product, ct);

            return ApiResponse<int>.Ok(result, "Product created successfully.");

        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid product ID.");

            var prd = await _repo.GetProductByIdAsync(id, ct);
            if (prd == null)
                throw new NotFoundException("Product", id);

            var result = await _repo.DeleteProductAsync(id, ct);
            if (!result)
                return ApiResponse<bool>.Failed("Product not found or could not be deleted.");

            return ApiResponse<bool>.Ok(true, "Product deleted successfully.");
        }


        public async Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetAllProductsAsync(CancellationToken ct)
        {
            var products = await _repo.GetAllProductsAsync(ct);

            var mappedProducts = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            if (!mappedProducts.Any())
                return ApiResponse<IEnumerable<ProductResponseDto>>
                    .Ok(mappedProducts, "No products found.");

            return ApiResponse<IEnumerable<ProductResponseDto>>
                .Ok(mappedProducts, "Products retrieved successfully.");
        }
        public async Task<ApiResponse<ProductResponseDto?>> GetProductByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid product ID.");

            var prd = await _repo.GetProductByIdAsync(id, ct);
            var mappedPrd = _mapper.Map<ProductResponseDto?>(prd);

            return ApiResponse<ProductResponseDto?>.Ok(mappedPrd, mappedPrd != null
                                                                  ? "Product retrieved successfully."
                                                                  : "Product not found.");
        }

        public async Task<ApiResponse<bool>> UpdateProductAsync(UpdateProductDto dto, CancellationToken ct)
        {

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.ProductId <= 0)
                throw new ValidationException("Invalid product ID");

            var product = await _repo.GetProductByIdAsync(dto.ProductId, ct);

            if (product == null)
                throw new NotFoundException("Product", dto.ProductId);

            if (!string.IsNullOrEmpty(dto.SKU))
            {
                product.SetSKU(dto.SKU);

                if (!Regex.IsMatch(dto.SKU, @"^[A-Z]{3,5}-[A-Z0-9]{2,20}-\d{3,6}$"))
                    throw new ValidationException("Invalid SKU format");
            }

            _mapper.Map(dto, product);

            var updated = await _repo.UpdateProductAsync(product, ct);

            return updated
                ? ApiResponse<bool>.Ok(true, "Product updated successfully.")
                : ApiResponse<bool>.Failed("Failed to update product.");
        }
    }
}
