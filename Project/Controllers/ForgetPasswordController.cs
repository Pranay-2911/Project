using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly IForgetPasswordService _forgetPasswordService;
        public ForgetPasswordController(IForgetPasswordService forgetPasswordService)
        {
            _forgetPasswordService = forgetPasswordService;
        }

        [HttpPost]
        public IActionResult Request(ForgetPasswordRequestDto requestDto)
        {
            var email = _forgetPasswordService.ChnagePasswordRequest(requestDto);
            return Ok(new { email = email});
        }

        [HttpPut]
        public IActionResult ChangePassword(ForgetPasswordDto requestDto)
        {
            var id = _forgetPasswordService.ChangePassword(requestDto);
            return Ok(id);
        }
    }
}
