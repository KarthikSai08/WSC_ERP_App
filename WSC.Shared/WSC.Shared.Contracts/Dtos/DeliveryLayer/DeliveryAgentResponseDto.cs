namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public sealed class DeliveryAgentResponseDto
    {
        public int DeliveryAgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
