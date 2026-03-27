
using System.ComponentModel.DataAnnotations;
using WSC.Shared.Contracts.ValueObjects;

namespace WSC.CRM.Domain.Entities
{
    public class Customer
    {
        [Key]
        public int CxId { get; set; }
        [Required]
        [StringLength(50)]
        public string CxName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string CxEmail { get; set; } = null!;
        public string CxPhone { get; set; }
        public bool IsActive { get; set; } = true;
        public Address? CxAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Lead> Leads { get; set; }
        public ICollection<Opportunity> Opportunities { get; set; }
    }
}
