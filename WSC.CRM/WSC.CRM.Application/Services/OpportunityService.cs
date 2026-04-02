using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.CRM.Application.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly IOpportunityRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _cxRepo;
        public OpportunityService(IOpportunityRepository repo, IMapper mapper, ICustomerRepository cxRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _cxRepo = cxRepo;
        }
        public async Task<ApiResponse<int>> CreateOpportunityAsync(CreateOpportunityDto dto, CancellationToken ct)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            var cxExists = await _cxRepo.GetCustomerByIdAsync(dto.CustomerId, ct);
            if (cxExists is null)
                throw new NotFoundException("Customer", dto.CustomerId);

            if(!cxExists.IsActive)
                throw new InActiveException("Customer", dto.CustomerId);

            var opp = _mapper.Map<Opportunity>(dto);

            var created =await _repo.CreateOpportunityAsync(opp, ct);
            if(created <= 0)
                return ApiResponse<int>.Failed("Failed to create opportunity");
           
            return ApiResponse<int>.Ok(created, "Opportunity created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteOpportunityAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(id));

            var deleted =await _repo.DeleteOpportunityAsync(id, ct);
            
            return deleted ? ApiResponse<bool>.Ok(true, "Opportunity deleted successfully") 
                    : ApiResponse<bool>.Failed("Failed to delete opportunity");
        }

        public Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetAllAOpportunitiesByFilterAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetAllOpportunitiesAsync(CancellationToken ct)
        {
            var opp = await _repo.GetAllOpportunitiesAsync(ct);

            var mappedOpportunities = opp != null
                ? _mapper.Map<IEnumerable<OpportunityResponseDto>>(opp)
                : Enumerable.Empty<OpportunityResponseDto>();

            return ApiResponse<IEnumerable<OpportunityResponseDto>>
                .Ok(mappedOpportunities, "Opportunities retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct)
        {
            if(cxId <= 0)
                throw new ArgumentException("Invalid customer id", nameof(cxId));

            var opp =await _repo.GetOpportunitiesByCustomerIdAsync(cxId, ct);

            var mappedOpportunities = opp != null
                ? _mapper.Map<IEnumerable<OpportunityResponseDto>>(opp)
                : Enumerable.Empty<OpportunityResponseDto>();

            return ApiResponse<IEnumerable<OpportunityResponseDto>>
                .Ok(mappedOpportunities, "Opportunities retrieved successfully");
        }

        public async Task<ApiResponse<OpportunityResponseDto?>> GetOpportunityByIdAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(id));

            var opp =await _repo.GetOpportunityByIdAsync(id, ct);
            if (opp == null)
                throw new NotFoundException("Opportunity" , id);

            var mappedOpportunity = _mapper.Map<OpportunityResponseDto>(opp);
            return ApiResponse<OpportunityResponseDto?>.Ok(mappedOpportunity, "Opportunity retrieved successfully");
        }

        public async Task<ApiResponse<Opportunity?>> GetOpportunityEntityByIdAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(id));

            var opp =await _repo.GetOpportunityEntityByIdAsync(id, ct);
            var mappedOpportunity = opp != null
                ? _mapper.Map<Opportunity>(opp)
                : null;

            return ApiResponse<Opportunity?>.Ok(mappedOpportunity, "Opportunity retrieved successfully");
        }

        public async Task<ApiResponse<PagedResponse<OpportunityResponseDto>>> GetPagedOpportunitiesAsync(PaginationRequest request, CancellationToken ct)
        {
            var (data, totalCount) = await _repo.GetPagedOpportunitiesAsync(request, ct);

            var mappedData = _mapper.Map<IEnumerable<OpportunityResponseDto>>(data);
            var response = new PagedResponse<OpportunityResponseDto>
            (
                mappedData,
                request.PageSize,
                request.PageNumber,
                totalCount
            );

            return ApiResponse<PagedResponse<OpportunityResponseDto>>
                .Ok(response, "Paged Opportunities retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateClosedAtAsync(int oppId, CancellationToken ct)
        {
            if (oppId <= 0) throw new ArgumentException(nameof(oppId));

            var opp = await _repo.GetOpportunityEntityByIdAsync(oppId, ct);
            if (opp == null)
                throw new NotFoundException("Opportunity", oppId);

            if (opp.ClosedAt != null)
                throw new ValidationException("Opportunity already closed");

            var updated =await _repo.UpdateClosedAtAsync(oppId, ct);

                
            return ApiResponse<bool>.Ok(updated, updated ? "Opportunity closed successfully" : "Failed to close opportunity");  

        }

        public async Task<ApiResponse<bool>> UpdateOpportunityAsync(UpdateOpportunityDto dto, CancellationToken ct)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            if(dto.OpportunityId <= 0)
                throw new ArgumentException("Invalid opportunity id", nameof(dto.OpportunityId));

            var opp = await _repo.GetOpportunityEntityByIdAsync(dto.OpportunityId, ct);
            if(opp == null)
                throw new NotFoundException("Opportunity", dto.OpportunityId);
            
            _mapper.Map(dto, opp);
            var updated =await _repo.UpdateOpportunityAsync(opp, ct);

            return updated 
                    ? ApiResponse<bool>.Ok(true, "Opportunity updated successfully") 
                    : ApiResponse<bool>.Failed("Failed to update opportunity");
        }
    }
}
