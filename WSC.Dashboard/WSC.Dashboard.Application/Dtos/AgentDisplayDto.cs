namespace WSC.Dashboard.Application.Dtos
{
    public sealed record AgentDisplayDto
    {
        public int DeliveryAgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }
    }
}
