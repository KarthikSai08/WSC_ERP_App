using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Dtos
{
    public class UpdateOpportunityDto
    {
        public int OpportunityId { get; set; }
        public string? OpportunityName { get; set; }
        public OpportunityStage? Stage { get; set; }
        public decimal? Amount { get; set; }
    }
}
