namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class CreateDeliveryAgentDto
    {
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
