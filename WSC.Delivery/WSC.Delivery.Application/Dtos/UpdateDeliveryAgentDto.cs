namespace WSC.Delivery.Application.Dtos
{
    public sealed record UpdateDeliveryAgentDto(
        int AgentId,
        string Name,
        string Phone,
        string Email,
        string VehicleNumber,
        string LicenseNumber,
        bool IsActive);
}
