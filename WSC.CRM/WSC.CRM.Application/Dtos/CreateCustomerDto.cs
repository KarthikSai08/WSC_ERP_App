using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WSC.Shared.Contracts.ValueObjects;

namespace WSC.CRM.Application.Dtos
{
    public class CreateCustomerDto
    {
        [Required]
        [StringLength(50)]
        public string CxName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string CxEmail { get; set; } = null!;

        public string CxPhone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}
