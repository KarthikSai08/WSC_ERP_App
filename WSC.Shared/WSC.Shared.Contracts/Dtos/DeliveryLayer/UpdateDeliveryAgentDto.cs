namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class UpdateDeliveryAgentDto
    {
        public int DeliveryAgentId { get; set; }
        public string? AgentName { get; set; }
        public string? AgentPhone { get; set; }
        public string? VehicleNumber { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
