using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken ct);
        Task<Customer?> GetCustomerById(int id, CancellationToken ct);
        Task<int> CreateCustomerAsync(Customer cx, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<bool> UpdateCustomerAsync(Customer cx, CancellationToken ct);
        Task<bool> DeleteCustomerAsync(int id, CancellationToken ct);
    }
}
