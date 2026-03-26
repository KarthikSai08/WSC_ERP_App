
namespace WSC.CRM.Domain.Entities
{
    public class Lead
    {
        public int LeadId { get; set; }
        public string LeadName { get; set; }
        public string LeadEmail { get; set; }
        public string LeadPhone { get; set; }

        public LeadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
