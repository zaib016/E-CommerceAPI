using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        //[ForeignKey("UserId")]
        //public User User { get; set; }
        public int UserId { get; set; }
        //[ForeignKey("ProductId")]
        //public Product Product { get; set; }
        public int ProductId { get; set; }
        public string TotalAmount { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
    }
}
