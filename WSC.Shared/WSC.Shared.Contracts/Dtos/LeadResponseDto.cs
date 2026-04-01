using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.Shared.Contracts.Dtos
{
    public class LeadResponseDto
    {
        public int LeadId { get; set; }
        public string LeadName { get; set; }
        public string LeadPhone { get; set; }

        public LeadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsActive { get; set; }

    }
}
