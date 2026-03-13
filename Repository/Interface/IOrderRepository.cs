using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<Order?> getOrderByOrderIdAsync(int id);
        Task<Order?> getOrderByUserIdAsync(int id);
        Task<Order> addAsync(Order order);
    }
}
