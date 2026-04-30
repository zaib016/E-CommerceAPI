using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repository
{
    public class OrderRepository : DbProvider, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Order> addAsync(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
        
        public async Task<List<OrderDTOs>> getAll()
        {
            //var orders = await _dbContext.Orders
            //    .Where(o => o.UserId == 1)
            //    .ToListAsync();
            //*********** Payload Trimming *************//
            var orders = await _dbContext.Orders
                .Select(o => new OrderDTOs
                {
                    ProductId = o.ProductId,
                    UserId = o.UserId,
                    TotalAmount = o.TotalAmount

                }).ToListAsync();

            return orders;
        }

        public async Task<Order?> getOrderByOrderIdAsync(int id)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<Order?> getOrderByUserIdAsync(int id)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(u => u.UserId == id);
        }
    }
}
