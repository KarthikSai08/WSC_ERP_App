using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.CRM.Domain.Entities
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ActivityType Type { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int LeadId { get; set; }
        public Lead Lead { get; set; }
    }
}
