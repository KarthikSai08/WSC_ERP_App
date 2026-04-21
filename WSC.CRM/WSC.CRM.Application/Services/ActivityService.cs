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
    public sealed class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repo;
        private readonly ILeadRepository _leadRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ActivityService> _logger;
        private readonly IRedisCacheService _cache;
        public ActivityService(IActivityRepository repo, IMapper mapper, ILeadRepository leadRepo, ILogger<ActivityService> logger, IRedisCacheService cache)
        {
            _mapper = mapper;
            _repo = repo;
            _leadRepo = leadRepo;
            _logger = logger;
            _cache = cache;
        }
        public async Task<ApiResponse<int>> CreateActivityAsync(CreateActivityDto dto, CancellationToken ct)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.LeadId.HasValue)
            {
                var lead = await _leadRepo.GetLeadByIdAsync(dto.LeadId.Value, ct);
                if (lead == null)
                    throw new NotFoundException("Lead", dto.LeadId);
                if (!lead.IsActive)
                    throw new InActiveException("Lead", dto.LeadId);
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ValidationException("Title is Required !");

            var act = _mapper.Map<Activity>(dto);
            var activityId = await _repo.CreateActivityAsync(act, ct);

            if (activityId <= 0)
                throw new InvalidInputIdException(activityId);

            await _cache.RemoveAsync("Activities:All");
            await _cache.RemoveAsync($"Activities:Lead:{dto.LeadId}");
            await _cache.RemoveAsync($"Activities:Page:1:Size:10");

            _logger.LogInformation("Activity with ID {ActivityId} created successfully", activityId);
            return ApiResponse<int>.Ok(activityId, "Activity created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteActivityAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new InvalidInputIdException(id);

            var deleted = await _repo.DeleteActivityAsync(id, ct);

            _logger.LogInformation("Attempt to delete activity with ID {ActivityId} resulted in {Result}", id, deleted ? "success" : "failure");

            await _cache.RemoveAsync($"Activity:{id}");
            await _cache.RemoveAsync("Activities:All");

            return deleted
                ? ApiResponse<bool>.Ok(true, "Activity deleted successfully")
                : ApiResponse<bool>.Failed("Failed to delete activity");
        }

        public async Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetActivitiesByLeadIdAsync(int leadId, CancellationToken ct)
        {

            if (leadId <= 0)
                throw new InvalidInputIdException(leadId);

            var cacheKey = $"Activities:Lead:{leadId}";

            var cached = await _cache.GetAsync<IEnumerable<ActivityResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Cache hit for activities of lead ID {LeadId}", leadId);
                return ApiResponse<IEnumerable<ActivityResponseDto>>.Ok(cached, "Activities retrieved successfully (from cache)");
            }

            var activities = await _repo.GetActivitiesByLeadIdAsync(leadId, ct);
            if (activities == null || !activities.Any())
                return ApiResponse<IEnumerable<ActivityResponseDto>>.Failed("No activities found for the given lead ID");

            var mappedActivities = _mapper.Map<IEnumerable<ActivityResponseDto>>(activities);

            await _cache.SetAsync(cacheKey, mappedActivities, TimeSpan.FromMinutes(15));
            return ApiResponse<IEnumerable<ActivityResponseDto>>.Ok(mappedActivities, "Activities retrieved successfully");
        }

        public async Task<ApiResponse<ActivityResponseDto?>> GetActivityByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new InvalidInputIdException(id);

            var cacheKey = $"Activity:{id}";

            var cached = await _cache.GetAsync<ActivityResponseDto?>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Cache hit for activity ID {ActivityId}", id);
                return ApiResponse<ActivityResponseDto?>.Ok(cached, "Activity retrieved successfully (from cache)");
            }

            var activity = await _repo.GetActivityByIdAsync(id, ct);
            if (activity == null)
                return ApiResponse<ActivityResponseDto?>.Failed("Activity not found");

            var mappedActivity = _mapper.Map<ActivityResponseDto?>(activity);

            await _cache.SetAsync(cacheKey, mappedActivity, TimeSpan.FromMinutes(15));
            return ApiResponse<ActivityResponseDto?>.Ok(mappedActivity, "Activity retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetAllActivitiesAsync(CancellationToken ct)
        {
            var cacheKey = "Activities:All";

            var cached = await _cache.GetAsync<IEnumerable<ActivityResponseDto>>(cacheKey);

            if (cached != null)
            {
                _logger.LogInformation("Cache hit for all activities");
                return ApiResponse<IEnumerable<ActivityResponseDto>>.Ok(cached, "Activities retrieved successfully (from cache)");
            }

            var activities = await _repo.GetAllActivitiesAsync(ct);

            var mappedActivities = activities != null
                ? _mapper.Map<IEnumerable<ActivityResponseDto>>(activities)
                : Enumerable.Empty<ActivityResponseDto>();

            await _cache.SetAsync(cacheKey, mappedActivities, TimeSpan.FromMinutes(15));
            return ApiResponse<IEnumerable<ActivityResponseDto>>
                .Ok(mappedActivities, "Activities retrieved successfully");
        }

        public Task<ApiResponse<IEnumerable<ActivityResponseDto>>> GetAllActivitiesByFilterAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<PagedResponse<ActivityResponseDto>>> GetPagedActivitiesAsync(PaginationRequest request, CancellationToken ct)
        {
            var cacheKey = $"Activities:Page:{request.PageNumber}:Size:{request.PageSize}";
            var cached = await _cache.GetAsync<PagedResponse<ActivityResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Cache hit for paged activities: Page {PageNumber}, Size {PageSize}", request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<ActivityResponseDto>>.Ok(cached, "Paged activities retrieved successfully (from cache)");
            }

            var (data, totalCount) = await _repo.GetPagedActivitiesAsync(request, ct);

            var mappedData = _mapper.Map<IEnumerable<ActivityResponseDto>>(data);
            var response = new PagedResponse<ActivityResponseDto>
            (
                mappedData,
                request.PageSize,
                request.PageNumber,
                totalCount
            );

            await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(15));
            return ApiResponse<PagedResponse<ActivityResponseDto>>
                .Ok(response, "Paged activities retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateActivityAsync(UpdateActivityDto act, CancellationToken ct)
        {
            if (act == null)
                throw new ArgumentNullException(nameof(act));

            if (act.ActivityId <= 0)
                throw new InvalidInputIdException(act.ActivityId);

            var activity = await _repo.GetActivityEntityByIdAsync(act.ActivityId, ct);
            if (activity == null)
                throw new NotFoundException("Activity", act.ActivityId);

            _mapper.Map(act, activity);
            var updated = await _repo.UpdateActivityAsync(activity, ct);

            await _cache.RemoveAsync($"Activity:{act.ActivityId}");
            await _cache.RemoveAsync("Activities:All");
            await _cache.RemoveAsync($"Activities:Lead:{act.LeadId}");
            return updated
                ? ApiResponse<bool>.Ok(true, "Activity updated successfully")
                : ApiResponse<bool>.Failed("Failed to update activity");
        }

        public async Task<ApiResponse<bool>> UpdateCompletedAtAsync(int actId, CancellationToken ct)
        {
            if (actId <= 0)
                throw new InvalidInputIdException(actId);
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

            await _cache.RemoveAsync($"Activity:{actId}");
            await _cache.RemoveAsync("Activities:All");
            await _cache.RemoveAsync($"Activities:Lead:{act.LeadId}");
            await _cache.RemoveAsync($"Activities:Page:1:Size:10"); // Invalidate first page cache, ideally should be more dynamic

            return updated
                ? ApiResponse<bool>.Ok(true, "Activity completion status updated successfully")
                : ApiResponse<bool>.Failed("Failed to update activity completion status");

        }
    }
}
