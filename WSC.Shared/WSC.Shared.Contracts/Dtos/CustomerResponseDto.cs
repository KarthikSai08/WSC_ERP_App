using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace WSC.Shared.Contracts.Dtos
{
    public class CustomerResponseDto
    {
        public int CxId { get; set; }
        public string CxName { get; set; }
        public string CxPhone { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
