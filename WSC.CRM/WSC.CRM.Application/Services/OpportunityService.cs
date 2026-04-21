using AutoMapper;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.CRM.Application.Services
{
    public sealed class OpportunityService : IOpportunityService
    {
        private readonly IOpportunityRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _cxRepo;
        private readonly ILogger<OpportunityService> _logger;
        private readonly IRedisCacheService _cache;
        public OpportunityService(IOpportunityRepository repo, IMapper mapper, ICustomerRepository cxRepo, ILogger<OpportunityService> logger, IRedisCacheService cache)

        {
            _mapper = mapper;
            _repo = repo;
            _cxRepo = cxRepo;
            _logger = logger;
            _cache = cache;
        }
        public async Task<ApiResponse<int>> CreateOpportunityAsync(CreateOpportunityDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.CustomerId.HasValue)
            {
                var cxExists = await _cxRepo.GetCustomerByIdAsync(dto.CustomerId.Value, ct);
                if (cxExists is null)
                    throw new NotFoundException("Customer", dto.CustomerId);

                if (!cxExists.IsActive)
                    throw new InActiveException("Customer", dto.CustomerId);
            }

            var opp = _mapper.Map<Opportunity>(dto);

            var created = await _repo.CreateOpportunityAsync(opp, ct);
            if (created <= 0)
                return ApiResponse<int>.Failed("Failed to create opportunity");

            return ApiResponse<int>.Ok(created, "Opportunity created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteOpportunityAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(id));

            var deleted = await _repo.DeleteOpportunityAsync(id, ct);

            return deleted ? ApiResponse<bool>.Ok(true, "Opportunity deleted successfully")
                    : ApiResponse<bool>.Failed("Failed to delete opportunity");
        }

        public Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetAllAOpportunitiesByFilterAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetAllOpportunitiesAsync(CancellationToken ct)
        {
            var cacheKey = "Opportunities:All";
            var cached = await _cache.GetAsync<IEnumerable<OpportunityResponseDto>>(cacheKey);

            if (cached != null)
            {
                _logger.LogInformation("Opportunities retrieved from cache");
                return ApiResponse<IEnumerable<OpportunityResponseDto>>.Ok(cached, "Opportunities retrieved successfully (from cache)");
            }
            var opp = await _repo.GetAllOpportunitiesAsync(ct);

            var mappedOpportunities = opp != null
                ? _mapper.Map<IEnumerable<OpportunityResponseDto>>(opp)
                : Enumerable.Empty<OpportunityResponseDto>();

            await _cache.SetAsync(cacheKey, mappedOpportunities, TimeSpan.FromMinutes(15));
            return ApiResponse<IEnumerable<OpportunityResponseDto>>
                .Ok(mappedOpportunities, "Opportunities retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct)
        {
            if (cxId <= 0)
                throw new ArgumentException("Invalid customer id", nameof(cxId));

            var cacheKey = $"Opportunities:Customer:{cxId}";
            var cached = await _cache.GetAsync<IEnumerable<OpportunityResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Opportunities for customer {CustomerId} retrieved from cache", cxId);
                return ApiResponse<IEnumerable<OpportunityResponseDto>>.Ok(cached, "Opportunities retrieved successfully (from cache)");
            }

            var opp = await _repo.GetOpportunitiesByCustomerIdAsync(cxId, ct);

            var mappedOpportunities = opp != null
                ? _mapper.Map<IEnumerable<OpportunityResponseDto>>(opp)
                : Enumerable.Empty<OpportunityResponseDto>();

            await _cache.SetAsync(cacheKey, mappedOpportunities, TimeSpan.FromMinutes(15));
            return ApiResponse<IEnumerable<OpportunityResponseDto>>
                .Ok(mappedOpportunities, "Opportunities retrieved successfully");
        }

        public async Task<ApiResponse<OpportunityResponseDto?>> GetOpportunityByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(id));

            var cacheKey = $"Opportunity:{id}";
            var cached = await _cache.GetAsync<OpportunityResponseDto?>(cacheKey);

            if (cached != null)
            {
                _logger.LogInformation("Opportunity {OpportunityId} retrieved from cache", id);
                return ApiResponse<OpportunityResponseDto?>.Ok(cached, "Opportunity retrieved successfully (from cache)");
            }

            var opp = await _repo.GetOpportunityByIdAsync(id, ct);
            if (opp == null)
                throw new NotFoundException("Opportunity", id);

            var mappedOpportunity = _mapper.Map<OpportunityResponseDto>(opp);

            await _cache.SetAsync(cacheKey, mappedOpportunity, TimeSpan.FromMinutes(15));
            return ApiResponse<OpportunityResponseDto?>.Ok(mappedOpportunity, "Opportunity retrieved successfully");
        }

        public async Task<ApiResponse<Opportunity?>> GetOpportunityEntityByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(id));
            var cacheKey = $"OpportunityEntity:{id}";

            var cached = await _cache.GetAsync<Opportunity?>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Opportunity entity {OpportunityId} retrieved from cache", id);
                return ApiResponse<Opportunity?>.Ok(cached, "Opportunity retrieved successfully (from cache)");
            }

            var opp = await _repo.GetOpportunityEntityByIdAsync(id, ct);
            var mappedOpportunity = opp != null
                ? _mapper.Map<Opportunity>(opp)
                : null;

            await _cache.SetAsync(cacheKey, mappedOpportunity, TimeSpan.FromMinutes(15));
            return ApiResponse<Opportunity?>.Ok(mappedOpportunity, "Opportunity retrieved successfully");
        }

        public async Task<ApiResponse<PagedResponse<OpportunityResponseDto>>> GetPagedOpportunitiesAsync(PaginationRequest request, CancellationToken ct)
        {
            var cacheKey = $"Opportunities:Page:{request.PageNumber}:Size:{request.PageSize}";

            var cached = await _cache.GetAsync<PagedResponse<OpportunityResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Paged opportunities retrieved from cache (Page: {PageNumber}, Size: {PageSize})", request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<OpportunityResponseDto>>.Ok(cached, "Paged Opportunities retrieved successfully (from cache)");
            }
            var (data, totalCount) = await _repo.GetPagedOpportunitiesAsync(request, ct);

            var mappedData = _mapper.Map<IEnumerable<OpportunityResponseDto>>(data);
            var response = new PagedResponse<OpportunityResponseDto>
            (
                mappedData,
                request.PageSize,
                request.PageNumber,
                totalCount
            );
            await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(15));
            return ApiResponse<PagedResponse<OpportunityResponseDto>>.Ok(response, "Paged Opportunities retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateClosedAtAsync(int oppId, CancellationToken ct)
        {
            if (oppId <= 0) throw new ArgumentException(nameof(oppId));

            var opp = await _repo.GetOpportunityEntityByIdAsync(oppId, ct);
            if (opp == null)
                throw new NotFoundException("Opportunity", oppId);

            if (opp.ClosedAt != null)
                throw new ValidationException("Opportunity already closed");

            var updated = await _repo.UpdateClosedAtAsync(oppId, ct);


            return ApiResponse<bool>.Ok(updated, updated ? "Opportunity closed successfully" : "Failed to close opportunity");

        }

        public async Task<ApiResponse<bool>> UpdateOpportunityAsync(UpdateOpportunityDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.OpportunityId <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(dto.OpportunityId));

            var opp = await _repo.GetOpportunityEntityByIdAsync(dto.OpportunityId, ct);
            if (opp == null)
                throw new NotFoundException("Opportunity", dto.OpportunityId);

            _mapper.Map(dto, opp);
            var updated = await _repo.UpdateOpportunityAsync(opp, ct);

            return updated
                    ? ApiResponse<bool>.Ok(true, "Opportunity updated successfully")
                    : ApiResponse<bool>.Failed("Failed to update opportunity");
        }
    }
}
