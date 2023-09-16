using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;
using Project_API_NH.Dtos;
using Project_API_NH.Models;

using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Project_API_NH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        // Biến tĩnh để lưu trữ thông tin người dùng (đây chỉ là một ví dụ, không nên dùng biến tĩnh trong ứng dụng thực tế)
        //public static User user = new User();
        private readonly SalesNhContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SalesNhContext context ,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Hành động đăng ký người dùng
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(UserRegister userRequestDto)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == userRequestDto.Email);
            if (existingUser != null)
            {
                // Đã có người dùng sử dụng địa chỉ Email này, trả về lỗi hoặc thông báo tương ứng.
                return BadRequest("Email is already registered.");
            }

            // Mã hóa mật khẩu người dùng trước khi lưu trữ
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRequestDto.Password);

            // Tạo một đối tượng người dùng mới

            var u = new Models.User
            {
                Id = userRequestDto.Id,
                Name = userRequestDto.Name,
                Email = userRequestDto.Email,
                Password = passwordHash,
                Tel = userRequestDto.Tel

            };

            

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(u);
            _context.SaveChanges();

            // Trả về thông tin người dùng cùng với mã JWT (Token)

            return Ok(new UserData { Id = u.Id ,Name = u .Name, Email = u.Email, Token = GenerateJWT(u) });

        }

        // Tạo mã JWT dựa trên thông tin người dùng
        private string GenerateJWT(User user)
        {
            // Khóa bảo mật để ký và giải mã token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signatureKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Các thông tin (claims) về người dùng để đưa vào token
            var claims = new[]
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
               
            };

            // Tạo token với các thông tin trên
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],  // Người phát hành token
                _configuration["JWT:Audience"],  // Người nhận token
                claims,  // Danh sách thông tin người dùng (claims)
                expires: DateTime.Now.AddDays(2),  // Thời gian hết hạn của token
                signingCredentials: signatureKey  // Khóa để ký và giải mã token
            );
            // Trả về chuỗi token dưới dạng string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(UserLogin userLogin)
        {
            //Tìm User bằng emai
            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(userLogin.Email));

            if (user == null)
            {
                return Unauthorized("Email not found");
            }
            // Xác minh mật khẩu của người dùng bằng cách so sánh với mật khẩu đã mã hóa trong cơ sở dữ liệu

            bool verified = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);

            if (!verified)
            {
                return Unauthorized("Password is not true.");
            }

            // Trả về thông tin người dùng cùng với mã JWT (Token)
            return Ok(new UserData { Id = user.Id, Name = user.Name, Email = user.Email, Token = GenerateJWT(user) });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var Id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                var user = new UserData
                {
                    Id = Convert.ToInt32(Id),
                    Name = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value

                };
                return Ok(user);
            }
            return Unauthorized();
        }
    }
}
