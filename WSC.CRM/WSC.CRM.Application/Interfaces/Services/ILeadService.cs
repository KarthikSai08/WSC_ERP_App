using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Interfaces.Services
{
    public interface ILeadService
    {
        Task<ApiResponse<IEnumerable<LeadResponseDto>>> GetAllLeadsAsync(CancellationToken ct);
        Task<ApiResponse<LeadResponseDto?>> GetLeadByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<int>> CreateLeadAsync(CreateLeadDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateLeadAsync(UpdateLeadDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteLeadAsync(int id, CancellationToken ct);
        Task<ApiResponse<PagedResponse<LeadResponseDto>>> GetLeadsAsync(PaginationRequest request, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateLeadStatusAsync(int id, LeadStatus newStatus, CancellationToken ct);
    }
}
