using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Dashboard.Application.Dtos
{
    public class CustomerDisplayDto
    {
        public int CxId { get; set; }
        public string CxName { get; set; } = null!;
        public string CxEmail { get; set; } = null!;
        public string CxPhone { get; set; }

        public List<OrderDisplayDto> Orders { get; set; }
    }
}
