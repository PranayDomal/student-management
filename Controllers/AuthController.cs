using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentManagementAPI.Repository;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StudentRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(StudentRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            using (var con = _repo.GetCon())
            {
                con.Open();

                string query = "SELECT * FROM Users WHERE Username=@u AND Password=@p";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", model.Username);
                cmd.Parameters.AddWithValue("@p", model.Password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return Unauthorized();

                int userId = (int)reader["Id"];

                var token = GenerateToken(userId);

                return Ok(new { token });
            }
        }

        private string GenerateToken(int userId)
        {
            var keyString = _config["Jwt:Key"]
                            ?? throw new Exception("JWT Key missing in appsettings.json");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}