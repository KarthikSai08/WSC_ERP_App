using System.Text.Json;
using WSC.Gateway.Application.Interfaces.Clients;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.Gateway.Infrastructure.Clients
{
    public class CrmAggregatorClient : ICrmAggregatorClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public CrmAggregatorClient(HttpClient http) => _http = http;
        public async Task<IEnumerable<CustomerResponseDto>?> GetAllCustomerAsync(CancellationToken ct)
           => await GetAsync<IEnumerable<CustomerResponseDto>>("api/Customers/", ct);

        public async Task<IEnumerable<LeadResponseDto>?> GetAllLeadAsync(CancellationToken ct)
            => await GetAsync<IEnumerable<LeadResponseDto>>("api/Leads", ct);
        public async Task<IEnumerable<OpportunityResponseDto>?> GetAllOpportunityAsync(CancellationToken ct)
            => await GetAsync<IEnumerable<OpportunityResponseDto>>("api/Opportunities/GetAll", ct);

        private async Task<T?> GetAsync<T>(string url, CancellationToken ct)
        {
            var response = await _http.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode) return default;
            var content = await response.Content.ReadAsStringAsync(ct);
            var api = JsonSerializer.Deserialize<ApiResponse<T>>(content, _options);
            return api != null && api.Success ? api.Data : default;
        }
    }
}
