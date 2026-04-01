using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public class UpdateLeadDto
    {
        public string? LeadName { get; set; }
        public string? LeadEmail { get; set; }
        public string? LeadPhone { get; set; }
        public DateTime? UpdatedAt { get; set; } 
    }
}
