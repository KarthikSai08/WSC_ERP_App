using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Gateway.Application.Dtos
{
    public record AppSummaryDto
    {
        public UserProfileDto User { get; set; }
        public CrmSummaryDto Crm { get; set; }
        public StoreSummaryDto Store { get; set; }
        public DeliverySummaryDto Delivery { get; set; }
    }
}
