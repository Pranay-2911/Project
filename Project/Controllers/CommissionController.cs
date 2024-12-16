using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionRequestService _commissionRequestService;

        public CommissionController(ICommissionRequestService commissionRequestService)
        {
            _commissionRequestService = commissionRequestService;
        }

        [HttpGet("CommissionRequest"), Authorize(Roles = "ADMIN")]
        public IActionResult GetAllRequest([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var requests = _commissionRequestService.GetAllPendingRequest(pageParameter, ref count, searchQuery);
            return Ok(new { requests = requests, count = count });
        }
        [HttpPut("CommissionRequest/Approve/{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Approve(Guid id)
        {
            if (_commissionRequestService.Approve(id))
            {
                return Ok(id);
            }
            return BadRequest();
        }
        [HttpPut("CommissionRequest/Reject/{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Reject(Guid id)
        {
            if (_commissionRequestService.Reject(id))
            {
                return Ok(id);
            }
            return BadRequest();
        }
    }
}
