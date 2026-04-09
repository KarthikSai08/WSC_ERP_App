using Grpc.Core;
using Microsoft.Extensions.Logging;
using WSC.Shared.Contracts.Protos;

namespace WSC.Store.API.Services;

public class CustomerGrpcClientService
{
    private readonly CustomerService.CustomerServiceClient _client;
    private readonly ILogger<CustomerGrpcClientService> _logger;

    public CustomerGrpcClientService(
        CustomerService.CustomerServiceClient client,
        ILogger<CustomerGrpcClientService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<CustomerResponse> GetCustomerAsync(int customerId)
    {
        try
        {
            _logger.LogInformation("=== Calling CRM gRPC for customer {CustomerId} ===", customerId);

            var response = await _client.GetCustomerAsync(new CustomerRequest
            {
                CustomerId = customerId
            });

            _logger.LogInformation("=== Got customer: {Name} ===", response.Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "=== gRPC FAILED for customer {CustomerId} ===", customerId);
            throw;
        }
    }
}