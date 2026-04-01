using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public class CreateLeadDto
    {
        public string LeadName { get; set; }
        public string LeadEmail { get; set; }
        public string LeadPhone { get; set; }

        public LeadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
    }
}
