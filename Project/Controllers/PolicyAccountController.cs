using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyAccountController : ControllerBase
    {
        private readonly IPolicyAccountService _policyAccountService;

        public PolicyAccountController(IPolicyAccountService policyAccountService)
        {
            _policyAccountService = policyAccountService;

        }

        [HttpGet("PolicyAccount"), Authorize(Roles = "ADMIN")]
        public IActionResult GetPolicyAccount([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery, [FromQuery] string? searchQuery1)
        {
            var count = 0;
            var accounts = _policyAccountService.GetAll(pageParameter, ref count, searchQuery, searchQuery1);
            return Ok(new { accounts = accounts, count = count });
        }


        [HttpGet("Claims"), Authorize(Roles = "ADMIN")]
        public IActionResult GetAllClaims([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var claims = _policyAccountService.GetAllClaims(pageParameter, ref count, searchQuery);
            return Ok(new { claims = claims, count = count });
        }

        [HttpPost("Approve/Claim/{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult ApproveClaim(Guid id)
        {
            if (_policyAccountService.ApproveClaims(id))
            {
                return Ok(id);
            }
            return BadRequest();

        }
    }
}
