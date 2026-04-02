using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Interfaces.Services
{
    public interface IActivityService
    {
        Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetAllActivitiesAsync(CancellationToken ct);
        Task<ApiResponse<ActivityResponseDto?>> GetActivityByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<int>> CreateActivityAsync(CreateActivityDto act, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateActivityAsync(UpdateActivityDto act, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteActivityAsync(int id, CancellationToken ct);
        Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetAllActivitiesByFilterAsync(CancellationToken ct);
        Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetActivitiesByLeadIdAsync(int leadId, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateCompletedAtAsync(int actId, CancellationToken ct);
        Task<ApiResponse<PagedResponse<ActivityResponseDto>>> GetPagedActivitiesAsync(PaginationRequest request, CancellationToken ct);
    }
}
