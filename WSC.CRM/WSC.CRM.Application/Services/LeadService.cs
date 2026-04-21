using AutoMapper;
using Microsoft.Extensions.Logging;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Enums;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.CRM.Application.Services
{
    public sealed class LeadService : ILeadService
    {
        private readonly IMapper _mapper;
        private readonly ILeadRepository _repo;
        private readonly ILogger<LeadService> _logger;
        private readonly IRedisCacheService _cache;
        public LeadService(IMapper mapper, ILeadRepository repo, ILogger<LeadService> logger, IRedisCacheService cache)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<int>> CreateLeadAsync(CreateLeadDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var exists = await _repo.ExistsByLeadAsync(dto.LeadEmail, ct);
            if (exists is true)
            {
                _logger.LogWarning("Attempt to create duplicate lead with email: {LeadEmail}", dto.LeadEmail);
                throw new DuplicateException("Lead", dto.LeadEmail);
            }

            var lead = _mapper.Map<Lead>(dto);
            var id = await _repo.CreateLeadAsync(lead, ct);

            _logger.LogInformation("Lead created with ID: {LeadId}", id);
            return ApiResponse<int>.Ok(id, "Lead created successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteLeadAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new InvalidInputIdException(id);

            var exists = await _repo.GetLeadByIdAsync(id, ct);
            if (exists == null)
            {
                _logger.LogWarning("Attempt to delete non-existent lead with ID: {LeadId}", id);
                throw new NotFoundException("Lead", id);
            }

            var res = await _repo.DeleteLeadAsync(id, ct);
            _logger.LogInformation("Lead with ID: {LeadId} deletion attempted. Success: {Success}", id, res);
            return res
                ? ApiResponse<bool>.Ok(true, "Lead deleted successfully.")
                : ApiResponse<bool>.Failed("Failed to delete lead.");
        }

        public async Task<ApiResponse<IEnumerable<LeadResponseDto>>> GetAllLeadsAsync(CancellationToken ct)
        {
            var cacheKey = "Leads:All";

            var cached = await _cache.GetAsync<IEnumerable<LeadResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Leads retrieved from cache. Count: {LeadCount}", cached.Count());
                return ApiResponse<IEnumerable<LeadResponseDto>>.Ok(cached, "Leads retrieved successfully from cache.");
            }


            var leads = await _repo.GetAllLeadsAsync(ct);
            if (leads == null || !leads.Any())
            {
                _logger.LogInformation("No leads found in the system.");
                return ApiResponse<IEnumerable<LeadResponseDto>>.Ok(Enumerable.Empty<LeadResponseDto>(), "No leads found.");
            }

            _logger.LogInformation("Retrieved {LeadCount} leads from the system.", leads.Count());

            await _cache.SetAsync(cacheKey, _mapper.Map<IEnumerable<LeadResponseDto>>(leads), TimeSpan.FromMinutes(15));
            return ApiResponse<IEnumerable<LeadResponseDto>>.Ok(
                _mapper.Map<IEnumerable<LeadResponseDto>>(leads),
                "Leads retrieved successfully.");
        }

        public async Task<ApiResponse<LeadResponseDto?>> GetLeadByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new InvalidInputIdException(id);

            var cacheKey = $"Lead:{id}";

            var cached = await _cache.GetAsync<LeadResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Lead retrieved from cache with ID: {LeadId} at {Time}", id, DateTime.UtcNow);
                return ApiResponse<LeadResponseDto>.Ok(cached, "Lead Found in cache!");
            }

            var lead = await _repo.GetLeadByIdAsync(id, ct);
            if (lead == null)
            {
                _logger.LogWarning("Lead with ID: {LeadId} not found.", id);
                throw new NotFoundException("Lead", id);
            }

            var result = _mapper.Map<LeadResponseDto>(lead);
            _logger.LogInformation("Lead with ID: {LeadId} retrieved successfully.", id);

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));
            return ApiResponse<LeadResponseDto>.Ok(result, "Lead Found!");
        }

        public async Task<ApiResponse<bool>> UpdateLeadAsync(UpdateLeadDto dto, CancellationToken ct)
        {
            var lead = await _repo.GetLeadEntityByIdAsync(dto.LeadId, ct);
            if (lead == null)
                throw new NotFoundException("Lead", dto.LeadId);

            _mapper.Map(dto, lead);
            var updated = await _repo.UpdateLeadAsync(lead, ct);

            return updated
                ? ApiResponse<bool>.Ok(true, "Lead updated successfully.")
                : ApiResponse<bool>.Failed("Failed to update lead.");
        }

        public async Task<ApiResponse<bool>> UpdateLeadStatusAsync(int id, LeadStatus newStatus, CancellationToken ct)
        {
            if (id <= 0)
                throw new InvalidInputIdException(id);

            if (!Enum.IsDefined(typeof(LeadStatus), newStatus))
                throw new ArgumentException("Invalid lead status.", nameof(newStatus));

            var updated = await _repo.UpdateLeadStatusAsync(id, newStatus, ct);
            _logger.LogInformation("Lead with ID: {LeadId} status update attempted to {NewStatus}. Success: {Success}", id, newStatus, updated);
            return updated
                ? ApiResponse<bool>.Ok(true, "Lead status updated successfully.")
                : ApiResponse<bool>.Failed("Failed to update lead status.");
        }
        public async Task<ApiResponse<PagedResponse<LeadResponseDto>>> GetLeadsAsync(PaginationRequest request, CancellationToken ct)
        {
            var cacheKey = $"Leads:Page:{request.PageNumber}:Size:{request.PageSize}";

            var cached = await _cache.GetAsync<PagedResponse<LeadResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Paged leads retrieved from cache. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<LeadResponseDto>>.Ok(cached, "Leads retrieved successfully from cache.");
            }
            var (data, totalCount) = await _repo.GetPagedLeadsAsync(request, ct);

            var response = new PagedResponse<LeadResponseDto>(
                data,
                request.PageNumber,
                request.PageSize,
                totalCount
            );

            await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(15));

            return ApiResponse<PagedResponse<LeadResponseDto>>
                .Ok(response, "Leads retrieved successfully.");
        }
    }
}
