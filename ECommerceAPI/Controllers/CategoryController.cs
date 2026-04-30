using System.Text.Json.Serialization;
using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private IGenericRepository<Category> _categoryRepo;
        private IMemoryCache _inMemoryCache;
        private IDistributedCache _distributedCache;

        public CategoryController(IGenericRepository<Category> categoryRepository, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _categoryRepo = categoryRepository;
            _inMemoryCache = memoryCache;
            _distributedCache = distributedCache;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var key = "categories_List";
            //In_Memory Cache
            if(!_inMemoryCache.TryGetValue(key, out List<Category> categories))
            {
                //Redis Cache
                var redisCache = await _distributedCache.GetStringAsync(key);
                if(redisCache != null)
                {
                    categories = JsonConvert.DeserializeObject<List<Category>>(redisCache);
                }
                else
                {
                    //DB
                    categories = await _categoryRepo.getAllAsync();
                    //Set Redis Cache
                    await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(categories), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }
                //Set In_Memory Cache
                _inMemoryCache.Set(key, categories, TimeSpan.FromMinutes(5));
            }
            //Http Cache
            Response.Headers["Cache-Control"] = "public,max-age=60";
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getById(int id)
        {
            var category = await _categoryRepo.getByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> addCategory(CategoryDTOs categoryDTOs)
        {
            var category = new Category
            {
                CategoryName = categoryDTOs.CategoryName,
            };

            await _categoryRepo.addAsync(category);
            return Ok(category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id ,CategoryDTOs categoryDTOs)
        {
            var category = await _categoryRepo.getByIdAsync(id);
            if (category == null) return NotFound();

            category.CategoryName = categoryDTOs.CategoryName;

            await _categoryRepo.updateAsync(category);
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var category = await _categoryRepo.deleteAsync(id);
            if (category == false) return NotFound();

            return Ok("Delete!");
        }
    }
}
