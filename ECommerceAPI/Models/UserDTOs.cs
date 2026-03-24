namespace ECommerceAPI.Models
{
    public class UserDTOs
    {
        public class RegisterDTOs
        {
            public required string Username { get; set; }
            public required string Email { get; set; }
            public required string HashPassword { get; set; }
        }
        public class LoginDTOs
        {
            public required string Email { get; set; }
            public required string HashPassword { get; set; }
        }
    }
}
