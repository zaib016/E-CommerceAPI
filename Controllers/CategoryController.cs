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
    public class CategoryController : ControllerBase
    {
        private IGenericRepository<Category> _categoryRepo;

        public CategoryController(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepo = categoryRepository; 
        }
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            return Ok(await _categoryRepo.getAllAsync());
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
