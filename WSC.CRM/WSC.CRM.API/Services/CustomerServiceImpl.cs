using Grpc.Core;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Protos;
using Microsoft.Extensions.Logging;

namespace WSC.CRM.API.Services
{
    public class CustomerServiceImpl : CustomerService.CustomerServiceBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerServiceImpl> _logger;

        public CustomerServiceImpl(
            ICustomerService customerService,
            ILogger<CustomerServiceImpl> logger)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<CustomerResponse> GetCustomer(
            CustomerRequest request,
            ServerCallContext context)
        {
            try
            {
                if (request == null || request.CustomerId <= 0)
                {
                    _logger.LogWarning("Invalid customer request. ID: {CustomerId}", request?.CustomerId);
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID"));
                }

                _logger.LogInformation("Fetching customer. ID: {CustomerId}", request.CustomerId);

                var result = await _customerService.GetByIdAsync(request.CustomerId);

                if (result?.Data == null || !result.Success)
                {
                    _logger.LogWarning("Customer not found. ID: {CustomerId}", request.CustomerId);
                    throw new RpcException(new Status(StatusCode.NotFound,
                        $"Customer {request.CustomerId} not found"));
                }

                _logger.LogInformation("Customer retrieved successfully. ID: {CustomerId}", request.CustomerId);

                return new CustomerResponse
                {
                    Id = result.Data.CxId,
                    Name = result.Data.CxName ?? "Unknown",
                    Phone = result.Data.CxPhone ?? string.Empty
                };
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer {CustomerId}", request?.CustomerId);
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while retrieving the customer"));
            }
        }
    }
}