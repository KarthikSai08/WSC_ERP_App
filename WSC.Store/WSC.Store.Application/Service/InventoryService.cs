using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public InventoryService(IInventoryRepository repo, IProductRepository productRepository, IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _repo = repo;
            _productRepo = productRepository;
            _uow = uow;
        }
        public async Task<ApiResponse<int>> CreateInventoryRecordAsync(CreateInventoryRecordDto record, CancellationToken ct)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            if (record.ProductId <= 0) throw new ValidationException("ProductId must be greater than 0");
            if (record.InStock < 0) throw new ValidationException("InStock cannot be negative");

            var prd = await _productRepo.GetProductByIdAsync(record.ProductId, ct);
            if (prd == null)
                throw new NotFoundException("ProductId ", record.ProductId);
            if (prd.IsActive == false)
                throw new ValidationException("Cannot create inventory record for inactive product");
            var exists = await _repo.RecordExistsByProductAsync(record.ProductId, ct);
            if (exists)
                throw new ValidationException("Inventory already exists for this product");
            if (record.MinStock < 0)
                throw new ValidationException("MinStock cannot be negative");

            if (record.MinStock > record.InStock)
                throw new ValidationException("MinStock cannot exceed InStock");
            var newRecord = _mapper.Map<Inventory>(record);
            var createdId = await _repo.CreateInventoryRecordAsync(newRecord, ct);

            return ApiResponse<int>.Ok(createdId, "Inventory record created successfully");

        }

        public async Task<ApiResponse<bool>> DeleteInventoryRecordAsync(int id, CancellationToken ct)
        {
            if (id <= 0) throw new ValidationException("Id must be greater than 0");

            var existingRecord = await _repo.GetInventoryRecordByIdAsync(id, ct);

            if (existingRecord == null)
                throw new NotFoundException("Inventory ", id);

            var deleted = await _repo.DeleteInventoryRecordAsync(id, ct);

            return deleted
                        ? ApiResponse<bool>.Ok(true, "Inventory record deleted successfully")
                        : ApiResponse<bool>.Failed("Inventory not found or already deleted");

        }

        public async Task<ApiResponse<IEnumerable<InventoryResponseDto>>> GetAllInventoryRecordsAsync(CancellationToken ct)
        {
            var records = await _repo.GetAllInventoryRecordsAsync(ct);

            var mappedRecords = _mapper.Map<IEnumerable<InventoryResponseDto>>(records);

            return ApiResponse<IEnumerable<InventoryResponseDto>>.Ok(mappedRecords, "Inventory records retrieved successfully");
        }

        public async Task<ApiResponse<InventoryResponseDto>> GetInventoryRecordByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0) throw new ValidationException("Id must be greater than 0");

            var record = await _repo.GetInventoryRecordByIdAsync(id, ct);
            if (record == null)
                throw new NotFoundException("Inventory ", id);

            var mappedRecord = _mapper.Map<InventoryResponseDto>(record);
            return ApiResponse<InventoryResponseDto>.Ok(mappedRecord, "Inventory record retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateStockAsync(int id, int quantity, CancellationToken ct)
        {
            if (id <= 0)
                throw new ValidationException("Id must be greater than 0");
            if (quantity < 0)
                throw new ValidationException("Quantity cannot be negative");

            var existingRecord = await _repo.GetInventoryRecordByIdAsync(id, ct);
            if (existingRecord == null)
                throw new NotFoundException("Inventory ", id);


            existingRecord.InStock = quantity;
            var updated = await _repo.UpdateStockAsync(id, quantity,_uow.Transaction, ct);

            return ApiResponse<bool>.Ok(updated, "Stock updated successfully");
        }
    }
}
