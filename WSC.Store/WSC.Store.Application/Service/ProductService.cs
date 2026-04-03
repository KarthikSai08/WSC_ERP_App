using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.Application.Service
{
    public class ProductService : IProductService
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
            if(dto == null)
                throw new ArgumentNullException("Product data cannot be null.");

            var regex = new Regex(@"^[A-Z]{3,5}-[A-Z0-9]{2,20}-\d{3,6}$");

            if (!regex.IsMatch(dto.SKU))
                throw new ValidationException("Invalid SKU format");

            var product = _mapper.Map<Domain.Entities.Product>(dto);
            var result =await _repo.CreateProductAsync(product, ct);

            return ApiResponse<int>.Ok(result, "Product created successfully.");

        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid product ID.");

            var prd = await _repo.GetProductByIdAsync(id, ct);

            var result =await _repo.DeleteProductAsync(id, ct);
            if (!result)
                return ApiResponse<bool>.Failed("Product not found or could not be deleted.");

            return ApiResponse<bool>.Ok(true, "Product deleted successfully.");
        }

        public async Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetAllProductsAsync(CancellationToken ct)
        {
            var products =await _repo.GetAllProductsAsync(ct);
            if(products == null || !products.Any())
                return ApiResponse<IEnumerable<ProductResponseDto>>.Ok(new List<ProductResponseDto>(), "No products found.");

            var mappedProducts = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            return ApiResponse<IEnumerable<ProductResponseDto>>.Ok(mappedProducts, "Products retrieved successfully.");
        }

        public async Task<ApiResponse<ProductResponseDto?>> GetProductByIdAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid product ID.");

            var prd =await _repo.GetProductByIdAsync(id, ct);
            var mappedPrd = _mapper.Map<ProductResponseDto?>(prd);

            return ApiResponse<ProductResponseDto?>.Ok(mappedPrd, mappedPrd != null 
                                                                  ? "Product retrieved successfully."
                                                                  : "Product not found.");
        }

        public async Task<ApiResponse<bool>> UpdateProductAsync(UpdateProductDto prd, CancellationToken ct)
        {
            if(prd == null)
                throw new ArgumentNullException("Product data cannot be null.");
            
            if(prd.ProductId <= 0)
                throw new ArgumentException("Invalid product ID.");

            var product = await _repo.GetProductByIdAsync(prd.ProductId, ct);

            var mappedPrd = _mapper.Map(prd, product);

            return await _repo.UpdateProductAsync(mappedPrd, ct) 
                    ? ApiResponse<bool>.Ok(true, "Product updated successfully.")
                    : ApiResponse<bool>.Failed("Product not found or could not be updated.");
        }
    }
}
