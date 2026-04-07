using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.Shared.Contracts.Interfaces.CRMClients
{
    public interface ICustomerClient
    {
        Task<CustomerResponseDto?> GetCustomerByIdAsync(int customerId, CancellationToken ct);
    }
}
