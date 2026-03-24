namespace ECommerceAPI.Models
{
    public class ProductDTOs
    {
        public int CategoryId { get; set; }
        public required string ProductName { get; set; }
        public required string ImageUrl { get; set; }
        public required string Stock { get; set; }
        public required string Price { get; set; }
    }
}
