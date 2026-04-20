namespace WSC.Delivery.Application.Dtos
{
    public sealed class UpdateDeliveryAgentDto
    {
        public int AgentId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string VehicleNumber { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
