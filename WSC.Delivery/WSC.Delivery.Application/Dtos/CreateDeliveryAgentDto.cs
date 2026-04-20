namespace WSC.Delivery.Application.Dtos
{
    public sealed class CreateDeliveryAgentDto
    {
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }

    }
}
