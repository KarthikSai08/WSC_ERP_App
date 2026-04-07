using System.Text.Json;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Interfaces.CRMClients;

namespace WSC.Shared.Infrastructure.Clients
{
        public class CustomerClient : ICustomerClient
        {
            private readonly HttpClient _http;

            public CustomerClient(HttpClient http)
            {
                _http = http;
            }

            public async Task<CustomerResponseDto?> GetCustomerByIdAsync(int customerId, CancellationToken ct)
            {
                var response = await _http.GetAsync($"api/Customers/{customerId}", ct);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(ct);

                    if (string.IsNullOrWhiteSpace(content))
                        return null;

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<CustomerResponseDto>>(content, options);

                return apiResponse?.Data;
        }
        }
    }

