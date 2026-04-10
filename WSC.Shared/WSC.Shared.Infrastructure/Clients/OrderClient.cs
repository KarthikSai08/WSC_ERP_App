using System.Text.Json;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Interfaces.StoreClients;

namespace WSC.Shared.Infrastructure.Clients
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _http;
        public OrderClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<OrderResponseDto?> GetByOrderIdAsync(int orderId, CancellationToken ct)
        {
            var response = await _http.GetAsync($"api/Orders/order/{orderId}", ct);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
                return null;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<OrderResponseDto>>(content, options);

            return apiResponse?.Data;
        }
    }
}
