using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> getEmailAsync(string email);
        Task<User?> getUserIdAsync(int id);
        Task<string> registerAsync(UserDTOs.RegisterDTOs registerDTOs);
        Task<string> loginAsync(UserDTOs.LoginDTOs loginDTOs);
    }
    public interface ITokenService
    {
        string createTokenAsync(User user);
    }
    public interface IEmailService
    {
        Task sendEmailAsync(string toEmail, string subject, string body);
    }
}
