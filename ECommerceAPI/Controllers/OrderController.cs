using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private IOrderRepository _orderRepo;
        private IMemoryCache _cache;

        public OrderController(IOrderRepository orderRepository, IMemoryCache memoryCache)
        {
            _orderRepo = orderRepository;
            _cache = memoryCache;
        }
        //[HttpGet]
        //public async Task<IActionResult> getAllAsync()
        //{
        //    //**************In Memory Caching Implementation****************
        //    var key = "orderList";
        //    if(!_cache.TryGetValue(key, out List<OrderDTOs> orders))
        //    {
        //        orders = await _orderRepo.getAll();
        //        var cacheOptions = new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        //            SlidingExpiration = TimeSpan.FromMinutes(2)
        //        };
        //        _cache.Set(key, orders, cacheOptions);
        //    }

        //    return Ok(orders);
        //}
        [HttpPost]
        public async Task<IActionResult> addOrder(OrderDTOs orderDTOs)
        {
            var order = new Order
            {
                UserId = orderDTOs.UserId,
                ProductId = orderDTOs.ProductId,
                TotalAmount = orderDTOs.TotalAmount,
                //OrderTime = orderDTOs.OrderTime,
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
