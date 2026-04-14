using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Dashboard.Application.Dtos;
using WSC.Dashboard.Application.Interfaces.RepositoryInterfaces;
using WSC.Dashboard.Infrastructure.Persistence.Context;

namespace WSC.Dashboard.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _context;
        public DashboardRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<CustomerDisplayDto?> GetCustomerDashBoard(int cxId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT 
                        c.CxId, c.CxName, c.CxEmail, c.CxPhone,
                        o.OrderId, o.TotalAmount, o.Status as OrderStatus,
                        oi.OrderItemId, oi.ProductId, oi.ProductId, oi.Quantity, oi.UnitPrice, oi.TotalPrice,
                        d.DeliveryId, d.TrackingNumber, d.Status as DeliveryStatus, d.ScheduledDate,
                        a.DeliveryAgentId, a.AgentName, a.AgentPhone, a.VehicleNumber
                        
                        FROM crm.Customers c
                        LEFT JOIN store.Orders o ON c.CxId = o.CustomerId
                        LEFT JOIN store.OrderItems oi ON o.Orderd = oi.OrderId
                        LEFT JOIN delivery.OrderDeliveries d ON o.OrderId = d.OrderId
                        LEFT JOIN delivery,DeliveryAgents a ON d.AssignedAgentId = a.DeliveryAgentId
                        WHERE c.CustomerId = @CustomerId";

            var customerDict = new Dictionary<int, CustomerDisplayDto>();
            var orderDict = new Dictionary<int, OrderDisplayDto>();

            var result = await con.QueryAsync<CustomerDisplayDto, OrderDisplayDto, OrderItemsDisplayDto, DeliveryDisplayDto, AgentDisplayDto, CustomerDisplayDto>(
                sql,
                (customer, order, item, delivery, agent) =>
                {
                    if (!customerDict.TryGetValue(customer.CxId, out var existingCutsomer))
                    {
                        existingCutsomer = customer;
                        existingCutsomer.Orders = new List<OrderDisplayDto>();
                        customerDict.Add(existingCutsomer.CxId, existingCutsomer);
                    }

                    if (order != null && order.OrderId != 0)
                    {
                        if (!orderDict.TryGetValue(order.OrderId, out var existingOrder))
                        {
                            existingOrder = order;
                            existingOrder.OrderItems = new List<OrderItemsDisplayDto>();
                            existingOrder.Delivery = delivery;

                            if (delivery != null)
                            {
                                delivery.Agent = agent;
                            }

                            orderDict.Add(order.OrderId, existingOrder);
                            existingCutsomer.Orders.Add(existingOrder);
                        }

                        if (item != null && item.OrderItemId != 0)
                        {
                            existingOrder.OrderItems.Add(item);
                        }
                    }
                    return existingCutsomer;
                },
                new { CustomerId = cxId },
                splitOn: "OrderId, OrderItemId, DeliveryId, DeliveryAgentId"
                );
            return  customerDict.Values.FirstOrDefault();
        }
    }
}
