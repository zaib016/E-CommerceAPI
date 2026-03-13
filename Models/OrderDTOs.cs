namespace ECommerceAPI.Models
{
    public class OrderDTOs
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string TotalAmount { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
    }
}
