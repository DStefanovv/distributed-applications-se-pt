using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.Data;
using RestaurantReservation.DTOs;
using RestaurantReservation.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RestaurantReservationContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(RestaurantReservationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {

            var existingUser = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest("Потребител с този Email вече съществува.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                PasswordHash = passwordHash
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok("Успешна регистрация!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == request.Email);

            if (customer == null)
            {
                return Unauthorized("Невалиден Email или парола.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, customer.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Невалиден Email или парола.");
            }

            var token = GenerateJwtToken(customer);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Customer customer)
        {

            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiresMinutes = Convert.ToDouble(jwtSettings["ExpiresMinutes"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                new Claim("customerId", customer.CustomerId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
