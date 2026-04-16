using WSC.Dashboard.Application.Dtos;

namespace WSC.Dashboard.Application.Interfaces.RepositoryInterfaces
{
    public interface IDashboardRepository
    {
        Task<CustomerDisplayDto?> GetCustomerDashBoard(int cxId, CancellationToken ct);
    }
}
