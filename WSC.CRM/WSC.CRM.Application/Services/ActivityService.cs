using AutoMapper;
using System.ComponentModel.DataAnnotations;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.CRM.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repo;
        private readonly ILeadRepository _leadRepo;
        private readonly IMapper _mapper;
        public ActivityService(IActivityRepository repo, IMapper mapper, ILeadRepository leadRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _leadRepo = leadRepo;
        }
        public async Task<ApiResponse<int>> CreateActivityAsync(CreateActivityDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var lead = await _leadRepo.GetLeadByIdAsync(dto.LeadId, ct);
            if (lead == null)
                throw new NotFoundException("Lead", dto.LeadId);
            if (!lead.IsActive)
                throw new InActiveException("Lead", dto.LeadId);

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ValidationException("Title is Required !");

            var act = _mapper.Map<Activity>(dto);
            var activityId = await _repo.CreateActivityAsync(act, ct);

            if (activityId <= 0)
                throw new Exception("Failed to Create Activity");

            return ApiResponse<int>.Ok(activityId, "Activity created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteActivityAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new NotFoundException("Activity ", id);

            var deleted = await _repo.DeleteActivityAsync(id, ct);
            return deleted
                ? ApiResponse<bool>.Ok(true, "Activity deleted successfully")
                : ApiResponse<bool>.Failed("Failed to delete activity");
        }

        public async Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetActivitiesByLeadIdAsync(int leadId, CancellationToken ct)
        {
            if (leadId <= 0)
                throw new NotFoundException("Lead", leadId);

            var activities = await _repo.GetActivitiesByLeadIdAsync(leadId, ct);

            if (activities == null || !activities.Any())
                return ApiResponse<IEnumerable<ActivityResponseDto>>.Failed("No activities found for the given lead ID");

            var mappedActivities = _mapper.Map<IEnumerable<ActivityResponseDto>>(activities);
            return ApiResponse<IEnumerable<ActivityResponseDto>>.Ok(mappedActivities, "Activities retrieved successfully");

        }

        public async Task<ApiResponse<ActivityResponseDto?>> GetActivityByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new NotFoundException("Activity", id);

            var activity = await _repo.GetActivityByIdAsync(id, ct);
            if (activity == null)
                return ApiResponse<ActivityResponseDto?>.Failed("Activity not found");

            var mappedActivity = _mapper.Map<ActivityResponseDto?>(activity);

            return ApiResponse<ActivityResponseDto?>.Ok(mappedActivity, "Activity retrieved successfully");

        }

        public async Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetAllActivitiesAsync(CancellationToken ct)
        {
            var activities = await _repo.GetAllActivitiesAsync(ct);

            var mappedActivities = activities != null
                ? _mapper.Map<IEnumerable<ActivityResponseDto>>(activities)
                : Enumerable.Empty<ActivityResponseDto>();

            return ApiResponse<IEnumerable<ActivityResponseDto>>
                .Ok(mappedActivities, "Activities retrieved successfully");
        }

        public Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetAllActivitiesByFilterAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<PagedResponse<ActivityResponseDto>>> GetPagedActivitiesAsync(PaginationRequest request, CancellationToken ct)
        {
            var (data, totalCount) = await _repo.GetPagedActivitiesAsync(request, ct);

            var mappedData = _mapper.Map<IEnumerable<ActivityResponseDto>>(data);
            var response = new PagedResponse<ActivityResponseDto>
            (
                mappedData,
                request.PageSize,
                request.PageNumber,
                totalCount
            );

            return ApiResponse<PagedResponse<ActivityResponseDto>>
                .Ok(response, "Paged activities retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateActivityAsync(UpdateActivityDto act, CancellationToken ct)
        {
            if (act == null)
                throw new ArgumentNullException(nameof(act));

            if (act.ActivityId <= 0)
                throw new NotFoundException("Activity", act.ActivityId);

            var activity = await _repo.GetActivityEntityByIdAsync(act.ActivityId, ct);
            if (activity == null)
                throw new NotFoundException("Activity", act.ActivityId);

            _mapper.Map(act, activity);
            var updated = await _repo.UpdateActivityAsync(activity, ct);

            return updated
                ? ApiResponse<bool>.Ok(true, "Activity updated successfully")
                : ApiResponse<bool>.Failed("Failed to update activity");
        }

        public async Task<ApiResponse<bool>> UpdateCompletedAtAsync(int actId, CancellationToken ct)
        {
            if (actId <= 0)
                throw new NotFoundException("Activity", actId);
            var act = await _repo.GetActivityEntityByIdAsync(actId, ct);
            if (act == null)
                throw new NotFoundException("Activity", actId);

            if (act.ScheduledAt == null)
                throw new Exception("Scheduled At cannot be null");
            if (act.CompletedAt != null)
                throw new Exception("Activity is Already Completed");

            var now = DateTime.UtcNow;

            if (now < act.ScheduledAt)
                throw new ValidationException("Cannot complete before Scheduled time");

            var updated = await _repo.UpdateCompletedAtAsync(actId, ct);


            return updated
                ? ApiResponse<bool>.Ok(true, "Activity completion status updated successfully")
                : ApiResponse<bool>.Failed("Failed to update activity completion status");

        }
    }
}
