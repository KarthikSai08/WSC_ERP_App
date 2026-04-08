using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using WSC.Shared.Contracts.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Domain.Entities;
using WSC.Store.Infrastructure.Persistence.Context;

namespace WSC.Store.Infrastructure.Repository
{
	public class OrderItemsRepository : IOrderItemsRepository
	{
		private readonly DapperContext _context;
		public OrderItemsRepository(DapperContext context)
		{
			_context = context;
		}
		public async Task<int> CreateOrderItemAsync(OrderItems items,IDbTransaction transaction, CancellationToken ct)
		{
			var sql = @"INSERT INTO store.OrderItems
						(OrderId, ProductId, Quantity, UnitPrice, TotalPrice, CreatedAt, IsActive) 
						VALUES 
						(@OrderId, @ProductId, @Quantity, @UnitPrice, @TotalPrice, SYSUTCDATETIME(), 1);
						SELECT CAST(SCOPE_IDENTITY() as int)";

			var totalPrice = items.Quantity * items.UnitPrice;
			var parameters = new
			{
				items.OrderId,
				items.ProductId,
				items.Quantity,
				items.UnitPrice,
				TotalPrice = totalPrice
			};

			var orderItemId =await transaction.Connection.QuerySingleAsync<int>(new CommandDefinition(sql, parameters, transaction, cancellationToken: ct));
			return orderItemId;
		}

		public async Task<bool> DeleteOrderItemAsync(int orderItemId, CancellationToken ct)
		{
			using var con = _context.CreateConnection();
			var sql = @"UPDATE store.OrderItems
						SET IsActive = 0, UpdatedAt = SYSUTCDATETIME()
						WHERE OrderItemId = @Id";

			var parameters = new { Id = orderItemId };

			var affectedRows =await con.ExecuteAsync(new CommandDefinition( sql, parameters, cancellationToken: ct));	
			return affectedRows > 0;

        }

		public async Task<IEnumerable<OrderItemResponseDto>> GetAllOrderItemsAsync(CancellationToken ct)
		{
			using var con = _context.CreateConnection();
			var sql = @"SELECT oi.OrderItemId, oi.OrderId, oi.ProductId, oi.Quantity, oi.UnitPrice, oi.TotalPrice, p.ProductName
						FROM store.OrderItems oi
						LEFT JOIN store.Products p ON oi.ProductId = p.ProductId
						WHERE oi.IsActive = 1";

			var orderItems =await con.QueryAsync<OrderItemResponseDto>(new CommandDefinition(sql, cancellationToken: ct));
			return orderItems.ToList();
        }

		public async Task<OrderItemResponseDto> GetItemByIdAsync(int orderItemId, CancellationToken ct)
		{
			using var con = _context.CreateConnection();
			var sql = @"SELECT oi.OrderItemId, oi.OrderId, oi.ProductId, oi.Quantity, oi.UnitPrice, oi.TotalPrice, p.ProductName
						FROM store.OrderItems oi
						LEFT JOIN store.Products p ON oi.ProductId = p.ProductId
						WHERE oi.IsActive = 1 AND oi.OrderItemId = @Id";

			var parameters = new { Id = orderItemId };
			var orderItem =await con.QuerySingleOrDefaultAsync<OrderItemResponseDto>(new CommandDefinition(sql, parameters, cancellationToken: ct));

			return orderItem;

        }

		public async Task<OrderItems> GetOrderItemEntityByIdAsync(int orderId, CancellationToken ct)
		{
			using var con = _context.CreateConnection();
			var sql = @"SELECT OrderItemId, OrderId, ProductId, Quantity, UnitPrice
							FROM store.OrderItems	
							WHERE OrderItemId = @Id";

			var parameters = new { Id = orderId };
			var orderItems =await con.QuerySingleOrDefaultAsync<OrderItems>(new CommandDefinition(sql, parameters, cancellationToken: ct));
			return orderItems;

        }

		public async Task<bool> UpdateOrderItemAsync(OrderItems items, CancellationToken ct)
		{
			using var con = _context.CreateConnection();
			var sql = @"UPDATE store.OrderItems 
						SET UpdatedAt = SYSUTCDATETIME(),ProductId = @ProductId, Quantity = @Quantity, UnitPrice = @UnitPrice
						WHERE OrderItemId = @Id AND IsActive = 1";
			var parameters = new
			{
				Id = items.OrderItemId,
				items.ProductId,
				items.Quantity,
                items.UnitPrice
			};

			var affectedRows =await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));
			return affectedRows > 0;
        }
	}
}
