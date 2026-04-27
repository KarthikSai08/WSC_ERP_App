using Microsoft.Extensions.Logging;
using WSC.Gateway.Application.Dtos.AggregatorDtos;
using WSC.Gateway.Application.Interfaces;
using WSC.Gateway.Application.Interfaces.Clients;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Shared.Contracts.Dtos.StoreLayer;

namespace WSC.Gateway.Application.Services
{
    public class AggregatorService : IAggregatorService
    {
        private readonly ICrmAggregatorClient _crmClient;
        private readonly IStoreAggregatorClient _storeClient;
        private readonly IDeliveryAggregatorClient _deliveryClient;
        private readonly ILogger<AggregatorService> _logger;

        public AggregatorService(ICrmAggregatorClient crmClient,
                                IStoreAggregatorClient storeClient,
                                IDeliveryAggregatorClient deliveryClient,
                                ILogger<AggregatorService> logger)
        {
            _crmClient = crmClient;
            _storeClient = storeClient;
            _deliveryClient = deliveryClient;
            _logger = logger;
        }
        public async Task<ApiResponse<AppSummaryDto>> GetAppSummaryAsync(int userId, string role, CancellationToken ct)
        {
            _logger.LogInformation("Aggregator app summary for user {UserId} with role {Role}", userId, role);

            var summary = new AppSummaryDto();
            var crmSummary = new CrmSummaryDto();
            var storeSummary = new StoreSummaryDto();
            var deliverySummary = new DeliverySummaryDto();

            var tasks = new List<Task>();

            if(role is "Admin" or "SalesAgent")
            {
                tasks.Add(FetchCrmDataAsync(crmSummary, ct));
            }
            if(role is "Admin" or "SalesAgent")
            {
                tasks.Add(FetchStoreDataAsync(storeSummary, ct));
            }
            if(role is "Admin" or "DeliveryAgent")
            {
                tasks.Add(FetchDeliveryDataAsync(deliverySummary, ct));
            }

            await Task.WhenAll(tasks);
            summary.Crm = crmSummary;
            summary.Store = storeSummary;
            summary.Delivery = deliverySummary;

            return ApiResponse<AppSummaryDto>.Ok(summary, "APP Summary Retrived Successfully");
        }

        public async Task FetchCrmDataAsync(CrmSummaryDto dto, CancellationToken ct)
        {
            try
            {
                var customerTask = _crmClient.GetAllCustomerAsync(ct);
                var leadsTask = _crmClient.GetAllLeadAsync(ct);
                var opportunitiesTask = _crmClient.GetAllOpportunityAsync(ct);

                await Task.WhenAll(customerTask, leadsTask, opportunitiesTask);

                var customers = await customerTask ?? new List<CustomerResponseDto>();
                var leads = await leadsTask ?? new List<LeadResponseDto>();
                var opportunities = await opportunitiesTask ?? new List<OpportunityResponseDto>();

                dto.TotalCustomers = customers.Count();
                dto.TotalLeads = leads.Count();
                dto.TotalOpportunities = opportunities.Count();
                dto.RecentCustomers = customers.Take(5);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch CRM data for aggregation");
                throw;
            }
        }
        public async Task FetchStoreDataAsync(StoreSummaryDto dto, CancellationToken ct)
        {
            try
            {
                var ordersTask = _storeClient.GetAllOrdersAsync(ct);
                var productsTask = _storeClient.GetAllProductsAsync(ct);

                await Task.WhenAll(ordersTask, productsTask);

                var orders = await ordersTask ?? new List<OrderResponseDto>();
                var products = await productsTask ?? new List<ProductResponseDto>();

                dto.TotalOrders = orders.Count();
                dto.TotalProducts = products.Count();
                dto.RecentOrders = orders.Take(5);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch Store data for aggregation");
            }
        }
        public async Task FetchDeliveryDataAsync(DeliverySummaryDto dto, CancellationToken ct)
        {
            try
            {
                var deliveryTask = _deliveryClient.GetAllDeliveriesAsync(ct);
                var agentTask = _deliveryClient.GetAllAgentsAsync(ct);

                await Task.WhenAll(deliveryTask, agentTask);

                var deliveries = await deliveryTask ?? new List<OrderDeliveryResponseDto>();
                var agents = await agentTask ?? new List<DeliveryAgentResponseDto>();

                dto.TotalDeliveries = deliveries.Count();
                dto.ActiveAgents = agents.Count();
                dto.RecentDeliveries = deliveries.Take(5);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch Delivery Data for aggregator");
            }
        }

    }
}
