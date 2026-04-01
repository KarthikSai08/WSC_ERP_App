using WSC.CRM.Domain.Entities;

namespace WSC.CRM.Application.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken ct);
        Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken ct);
        Task<int> CreateCustomerAsync(Customer cx, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<bool> UpdateCustomerAsync(Customer cx, CancellationToken ct);
        Task<bool> DeleteCustomerAsync(int id, CancellationToken ct);
    }
}
