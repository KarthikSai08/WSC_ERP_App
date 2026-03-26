
using WSC.Shared.Contracts.ValueObjects;

namespace WSC.CRM.Domain.Entities
{
    public class Customer
    {
        public int CxId { get; set; }
        public string CxName { get; set; }
        public string CxEmail { get; set; }
        public string CxPhone { get; set; }

        public Address CxAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Lead> Leads { get; set; }
        public ICollection<Opportunity> Opportunities { get; set; }
    }
}
