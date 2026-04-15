using System.ComponentModel.DataAnnotations;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Domain.Entities
{
    public class Activity
    {
        public int ActivityId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public ActivityType Type { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsActive { get; set; }

        // Flexible linking (ONLY ONE should be set logically)
        public int? LeadId { get; set; }
        public int? OpportunityId { get; set; }
        public int? CustomerId { get; set; }
    }
}
