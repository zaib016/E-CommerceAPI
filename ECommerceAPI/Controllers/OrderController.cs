using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private IOrderRepository _orderRepo;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepo = orderRepository;
        }
        [HttpPost]
        public async Task<IActionResult> addOrder(OrderDTOs orderDTOs)
        {
            var order = new Order
            {
                UserId = orderDTOs.UserId,
                ProductId = orderDTOs.ProductId,
                TotalAmount = orderDTOs.TotalAmount,
                OrderTime = orderDTOs.OrderTime,
            };

            return Ok(await _orderRepo.addAsync(order));
        }
        [HttpGet("byOrder{id}")]
        public async Task<IActionResult> getByOrderId(int id)
        {
            var order = await _orderRepo.getOrderByOrderIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }
        [HttpGet("byUser{id}")]
        public async Task<IActionResult> getOrderByUserId(int id)
        {
            var order = await _orderRepo.getOrderByUserIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }
    }
}
