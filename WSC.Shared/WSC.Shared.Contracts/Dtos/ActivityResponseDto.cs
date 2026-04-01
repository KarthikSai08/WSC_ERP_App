using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.Shared.Contracts.Dtos
{
    public class ActivityResponseDto
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ActivityType Type { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsActive {  get; set; }
        public int LeadId { get; set; }
        public string LeadName { get; set; }
    }
}
