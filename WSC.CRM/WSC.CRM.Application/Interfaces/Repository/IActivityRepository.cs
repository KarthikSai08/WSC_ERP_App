using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Application.Interfaces.Repository
{
    public interface IActivityRepository
    {
        Task<IEnumerable<ActivityResponseDto>> GetAllActivitiesAsync(CancellationToken ct);
        Task<ActivityResponseDto?> GetActivityByIdAsync(int id, CancellationToken ct);
        Task<Activity?> GetActivityEntityByIdAsync(int id, CancellationToken ct);
        Task<int> CreateActivityAsync(Activity act, CancellationToken ct);
        Task<bool> UpdateActivityAsync(Activity act, CancellationToken ct);
        Task<bool> DeleteActivityAsync(int id, CancellationToken ct);
        Task<IEnumerable<ActivityResponseDto>> GetAllActivitiesByFilterAsync(CancellationToken ct);
        Task<IEnumerable<ActivityResponseDto>> GetActivitiesByLeadIdAsync(int leadId, CancellationToken ct);
        Task<bool> UpdateCompletedAtAsync(int actId, CancellationToken ct);
        Task<(IEnumerable<ActivityResponseDto> Data, int TotalCount)> GetPagedActivitiesAsync(PaginationRequest request, CancellationToken ct);
    }
}
