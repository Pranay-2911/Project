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
using System.Text.Json;


namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;


        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            
            var existingUser = _loginService.FindByUserName(loginDto.Username);
            if (existingUser != null)
            {
                if (BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.PasswordHash))
                {
                    var role = existingUser.Role.RoleName.ToString();
                    string token = "";
                    var user = _loginService.FindUser(role, existingUser.Id, ref token);
    
                    Response.Headers.Add("Jwt", token);
                    return Ok(new {userName = loginDto.Username, roleName = role });
                }
            }
            return BadRequest("Username or Password Doesnt match");
        }

        


    }

}
