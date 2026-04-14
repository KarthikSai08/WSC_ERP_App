using System;
using System.Collections.Generic;
using System.Text;
using WSC.Dashboard.Application.Dtos;
using WSC.Shared.Contracts.Common;

namespace WSC.Dashboard.Application.Interfaces.ServiceInterfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<CustomerDisplayDto?>> GetCustomerDashBoard(int cxId, CancellationToken ct);
    }
}
