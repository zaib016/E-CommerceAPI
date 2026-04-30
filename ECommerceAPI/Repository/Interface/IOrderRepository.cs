using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<List<OrderDTOs>> getAll();
        Task<Order?> getOrderByOrderIdAsync(int id);
        Task<Order?> getOrderByUserIdAsync(int id);
        Task<Order> addAsync(Order order);
    }
}
