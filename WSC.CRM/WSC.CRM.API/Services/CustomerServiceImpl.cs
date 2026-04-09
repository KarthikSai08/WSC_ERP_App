using Grpc.Core;
using WSC.Shared.Contracts.Protos;

namespace WSC.CRM.API.Services
{
    public class CustomerServiceImpl : CustomerService.CustomerServiceBase
    {
        public override Task<CustomerResponse> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CustomerResponse
            {
                CustomerId = request.CustomerId,
                Name = "Sai"
            });
        }
    }
}