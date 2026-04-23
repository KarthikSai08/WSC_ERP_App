using System.Text.Json;
using WSC.Gateway.Application.Interfaces.Clients;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Gateway.Infrastructure.Clients
{
    public class DeliveryAggregatorClient : IDeliveryAggregatorClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public async Task<IEnumerable<DeliveryAgentResponseDto>> GetAllAgentsAsync(CancellationToken ct)
            => await GetAsync<IEnumerable<DeliveryAgentResponseDto>>("api/DeliveryAgent/all-agents", ct);

        public async Task<IEnumerable<OrderDeliveryResponseDto>?> GetAllDeliveriesAsync(CancellationToken ct)
            => await GetAsync<IEnumerable<OrderDeliveryResponseDto>>("api/OrderDelivery/all-orderDelivery", ct);

        public async Task<T?> GetAsync<T>(string url, CancellationToken ct)
        {
            var response = await _http.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode) return default;

            var constant = await response.Content.ReadAsStringAsync(ct);
            var api = JsonSerializer.Deserialize<ApiResponse<T>>(constant, _options);
            return api != null && api.Success ? api.Data : default;
        }
    }
}
