using ECommerceAPI.Models;
using ECommerceAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepo;
        private IEmailService _emailRepo;

        public UserController( IUserRepository userRepository, IEmailService emailService)
        {
            _userRepo = userRepository;
            _emailRepo = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> register(UserDTOs.RegisterDTOs registerDTOs)
        {
            var user = await _userRepo.registerAsync(registerDTOs);
            if (user == null) return BadRequest();

            var subject = "Welcom Our App!";
            var body = $"{registerDTOs.Username},<br>Thank you for registering</br>";
            await _emailRepo.sendEmailAsync(registerDTOs.Email, subject, body);

            return Ok("Registerted Successfully: Email Send!");
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(UserDTOs.LoginDTOs loginDTOs)
        {
            var token = await _userRepo.loginAsync(loginDTOs);
            if (token == "Invalid email or password") return Unauthorized("Unuthorized");

            return Ok(token);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getUserById(int id)
        {
            var user = await _userRepo.getUserIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
