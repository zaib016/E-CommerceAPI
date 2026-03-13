using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.Models.Entities;
using ECommerceAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAPI.Repository
{
    public class UserRepository : DbProvider, IUserRepository, ITokenService, IEmailService
    {
        private IConfiguration _config;

        public UserRepository(ApplicationDbContext dbContext , IConfiguration configuration) : base(dbContext)
        {
            _config = configuration;
        }

        public string createTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                new Claim("username", user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    expires: DateTime.UtcNow.AddMinutes(10),
                    claims: claims,
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> getEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> getUserIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<string> loginAsync(UserDTOs.LoginDTOs loginDTOs)
        {
            var user = await getEmailAsync(loginDTOs.Email);
            if (user == null) return "Invalid email or password";

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTOs.HashPassword, user.Password);
            if (!isPasswordValid) return "Invalid email or password";

            return createTokenAsync(user);
        }

        public async Task<string> registerAsync(UserDTOs.RegisterDTOs registerDTOs)
        {
            var exists = await getEmailAsync(registerDTOs.Email);
            if (exists != null) return "User Already Exists";

            var user = new User
            {
                Username = registerDTOs.Username,
                Email = registerDTOs.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDTOs.HashPassword)
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return ("Registereted Successfully");
        }

        public async Task sendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = _config["EmailSetting:FromEmail"];
            var password = _config["EmailSetting:Password"];

            var mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using(var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(mail);
            };
        }
    }
}
