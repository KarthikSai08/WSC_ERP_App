using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Enums;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.CRM.Application.Services
{
    public class LeadService : ILeadService
    {
        private readonly IMapper _mapper;
        private readonly ILeadRepository _repo;
        public LeadService(IMapper mapper, ILeadRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ApiResponse<int>> CreateLeadAsync(CreateLeadDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var exists = await _repo.ExistsByLeadAsync(dto.LeadEmail, ct);
            if (exists is true)
                throw new DuplicateException("Lead", dto.LeadEmail);

            var lead = _mapper.Map<Lead>(dto);
            var id = await _repo.CreateLeadAsync(lead, ct);

            return ApiResponse<int>.Ok(id, "Lead created successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteLeadAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id");

            var exists = await _repo.GetLeadByIdAsync(id, ct);
            if (exists == null)
                throw new NotFoundException("Lead", id);

            var res = await _repo.DeleteLeadAsync(id, ct);
            return res
                ? ApiResponse<bool>.Ok(true, "Lead deleted successfully.")
                : ApiResponse<bool>.Failed("Failed to delete lead.");
        }

        public async Task<ApiResponse<IEnumerable<LeadResponseDto>>> GetAllLeadsAsync(CancellationToken ct)
        {
            var leads = await _repo.GetAllLeadsAsync(ct);
            if (leads == null || !leads.Any())
                return ApiResponse<IEnumerable<LeadResponseDto>>.Ok(Enumerable.Empty<LeadResponseDto>(), "No leads found.");

            return ApiResponse<IEnumerable<LeadResponseDto>>.Ok(
                _mapper.Map<IEnumerable<LeadResponseDto>>(leads),
                "Leads retrieved successfully.");
        }

        public async Task<ApiResponse<LeadResponseDto?>> GetLeadByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id");

            var lead = await _repo.GetLeadByIdAsync(id, ct);
            if (lead == null)
                throw new NotFoundException("Lead", id);

            var result = _mapper.Map<LeadResponseDto>(lead);
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
                throw new ArgumentOutOfRangeException("id");
            if (!Enum.IsDefined(typeof(LeadStatus), newStatus))
                throw new ArgumentException("Invalid lead status.", nameof(newStatus));

            var updated = await _repo.UpdateLeadStatusAsync(id, newStatus, ct);
            return updated
                ? ApiResponse<bool>.Ok(true, "Lead status updated successfully.")
                : ApiResponse<bool>.Failed("Failed to update lead status.");
        }
        public async Task<ApiResponse<PagedResponse<LeadResponseDto>>> GetLeadsAsync(PaginationRequest request, CancellationToken ct)
        {
            var (data, totalCount) = await _repo.GetPagedLeadsAsync(request, ct);

            var response = new PagedResponse<LeadResponseDto>(
                data,
                request.PageNumber,
                request.PageSize,
                totalCount
            );

            return ApiResponse<PagedResponse<LeadResponseDto>>
                .Ok(response, "Leads retrieved successfully.");
        }
    }
}
