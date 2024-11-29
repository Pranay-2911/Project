using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.DTOs;
using Project.Models;
using Project.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IConfiguration _configuration;
        public LoginController(ILoginService loginService, IConfiguration configuration)
        {
            _loginService = loginService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            var existingUser = _loginService.FindByUserName(loginDto.UserName);
            if (existingUser != null)
            {
                if (BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.PasswordHash))
                {
                    var role = existingUser.Role.RoleName.ToString();
                    var user = _loginService.FindUser(role, existingUser.Id);


                    //var token = CreateToken(user);
                    //Response.Headers.Add("Jwt", token);
                    return Ok(new { roleName = role });
                }
            }
            return BadRequest("Username or Password Doesnt match");
        }


        //private string CreateToken(Object user)
        //{

        //    var roleName = user.Role.RoleName.ToString();
        //    List<Claim> claim = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name,user.UserName),
        //        new Claim(ClaimTypes.Role, roleName)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
        //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        //    //token construction
        //    var token = new JwtSecurityToken(
        //        claims: claim,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: cred
        //        );
        //    //generate the token
        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwt;
        }
    
}
