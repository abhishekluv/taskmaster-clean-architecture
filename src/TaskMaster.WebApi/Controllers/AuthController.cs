using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using TaskMaster.Infrastructure.Identity;
using TaskMaster.WebApi.Dtos;

namespace TaskMaster.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser is not null)
            {
                return BadRequest(new { error = "Username is already taken." });
            }

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail is not null)
            {
                return BadRequest(new { error = "Email is already registered." });
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }

            // Optional: assign default role later

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            ApplicationUser? user;

            if (request.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            }

            if (user is null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                userName = User.Identity?.Name,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        private AuthResponse GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!));

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                ExpiresAt = expires,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            };
        }
    }
}
