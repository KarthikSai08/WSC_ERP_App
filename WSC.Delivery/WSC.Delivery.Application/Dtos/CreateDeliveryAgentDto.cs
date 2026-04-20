namespace WSC.Delivery.Application.Dtos
{
    public sealed record CreateDeliveryAgentDto(string AgentName, string AgentPhone, string VehicleNumber, string LicenseNumber);
}
