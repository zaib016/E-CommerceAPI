using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private IGenericRepository<Product> _productRepo;
        private readonly List<Product> productTable = new List<Product>();

        public ProductController(IGenericRepository<Product> genericRepository)
        {
            _productRepo = genericRepository;
            for(int i = 1; i <= 100; i++)
            {
                productTable.Add(new Product { CategoryId = i, ProductName = "", ImageUrl = "", Price = "", Stock = "" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            return Ok(await _productRepo.getAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getById(int id)
        {
            var product = await _productRepo.getByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> addProduct(ProductDTOs productDTOs)
        {
            var product = new Product
            {
                CategoryId = productDTOs.CategoryId,
                ProductName = productDTOs.ProductName,
                ImageUrl = productDTOs.ImageUrl,
                Stock = productDTOs.Stock,
                Price = productDTOs.Price,
            };
            await _productRepo.addAsync(product);
            return Ok(product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id , ProductDTOs productDTOs)
        {
            var product = await _productRepo.getByIdAsync(id);
            if (product == null) return NotFound();

            product.CategoryId = productDTOs.CategoryId;
            product.ProductName = productDTOs.ProductName;
            product.ImageUrl = productDTOs.ImageUrl;
            product.Stock = productDTOs.Stock;
            product.Price = productDTOs.Price;

            await _productRepo.updateAsync(product);
            return Ok(product);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var product = await _productRepo.deleteAsync(id);
            if (product == false) return NotFound();

            return Ok("Deleted!");
        }
        [HttpGet("pagination")]
        public async Task<IActionResult> getPages(int page = 1, int pageSize = 10)
        {
            var totalCount = productTable.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            var productPerPage = productTable.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(productPerPage);
        }
        [HttpPost("UploadImage")]
        public async Task<IActionResult> uploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No Image Uploaded");

            var folder = Path.Combine("wwwroot", "Image");
            if (!Directory.Exists(folder))Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, file.FileName);
            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/image/{file.FileName}";
            return Ok(new {imageUrl = url});
        }
    }
}
