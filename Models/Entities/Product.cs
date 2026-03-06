using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        //[ForeignKey("CategoryId")]
        //public Category Category { get; set; }
        public int CategoryId { get; set; }
        public required string ProductName { get; set; }
        public required string ImageUrl { get; set; }
        public required string Stock { get; set; }
        public required string Price { get; set; }
    }
}
