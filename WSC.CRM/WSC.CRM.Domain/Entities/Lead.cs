using System.ComponentModel.DataAnnotations;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Domain.Entities
{
    public class Lead
    {
        public int LeadId { get; set; }
        [Required]
        public string LeadName { get; set; }
        [Required]
        [EmailAddress]
        public string LeadEmail { get; set; }
        public string LeadPhone { get; set; }
        [Required]
        public LeadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

    }
}
