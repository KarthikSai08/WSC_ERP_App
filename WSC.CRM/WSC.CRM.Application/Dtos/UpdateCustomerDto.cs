using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.CRM.Application.Dtos
{
    public class UpdateCustomerDto
    {
        public int CxId { get; set; }
        public string? CxName { get; set; }
        public string? CxEmail { get; set; }
        public string? CxPhone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}
