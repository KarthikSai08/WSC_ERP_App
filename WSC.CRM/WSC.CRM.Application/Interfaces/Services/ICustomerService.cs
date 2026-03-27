using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Interfaces.Services
{
        public interface ICustomerService
        {
            Task<ApiResponse<int>> CreateCustomerAsync(CreateCustomerDto dto, CancellationToken ct);
            Task<ApiResponse<CustomerResponseDto?>> GetByIdAsync(int id, CancellationToken ct);
            Task<ApiResponse<IEnumerable<CustomerResponseDto>>> GetAllAsync(CancellationToken ct);
            Task<ApiResponse<bool>> UpdateCustomerAsync(UpdateCustomerDto dto, CancellationToken ct);
            Task<ApiResponse<bool>> DeleteCustomerAsync(int id, CancellationToken ct);
    }
}
