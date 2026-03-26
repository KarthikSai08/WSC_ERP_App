using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerResponseDto>> GetAllcustomersAsync(CancellationToken ct);
    }
}
