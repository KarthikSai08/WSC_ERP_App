using System.Text.Json;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Interfaces.StoreClients;

namespace WSC.Shared.Infrastructure.Clients
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _http;
        public ProductClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<ProductResponseDto?> GetProductByIdAsync(int productId, CancellationToken ct)
        {
            var response = await _http.GetAsync($"api/Products/GetById/{productId}", ct);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrEmpty(content))
                return null;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProductResponseDto>>(content, options);

            return apiResponse?.Data; ;
        }
    }
}
