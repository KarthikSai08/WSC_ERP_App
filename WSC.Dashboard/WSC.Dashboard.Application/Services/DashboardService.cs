using WSC.Dashboard.Application.Dtos;
using WSC.Dashboard.Application.Interfaces.RepositoryInterfaces;
using WSC.Dashboard.Application.Interfaces.ServiceInterfaces;
using WSC.Shared.Contracts.Common;

namespace WSC.Dashboard.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }
        public async Task<ApiResponse<CustomerDisplayDto?>> GetCustomerDashBoard(int cxId, CancellationToken ct)
        {
            if (cxId <= 0)
                throw new ArgumentException("Customer ID must be greater than 0.", nameof(cxId));

            var customerDashboard = await _repo.GetCustomerDashBoard(cxId, ct);

            if (customerDashboard == null)
                return ApiResponse<CustomerDisplayDto?>.Failed("Customer dashboard not found.");

            return ApiResponse<CustomerDisplayDto?>.Ok(customerDashboard, "Customer dashboard retrieved successfully.");
        }
    }
}
