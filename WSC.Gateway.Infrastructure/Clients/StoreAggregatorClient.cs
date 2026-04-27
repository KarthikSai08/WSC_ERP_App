using System.Text.Json;
using WSC.Gateway.Application.Interfaces.Clients;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;

namespace WSC.Gateway.Infrastructure.Clients
{
    public class StoreAggregatorClient : IStoreAggregatorClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public StoreAggregatorClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<OrderResponseDto>?> GetAllOrdersAsync(CancellationToken ct)
            => await GetAsync<IEnumerable<OrderResponseDto>>("api/Orders/all-orders", ct);

        public async Task<IEnumerable<ProductResponseDto>?> GetAllProductsAsync(CancellationToken ct)
            => await GetAsync<IEnumerable<ProductResponseDto>>("api/Products/all-products", ct);

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
