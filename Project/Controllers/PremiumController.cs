using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumService _premiumService;

        public PremiumController(IPremiumService premiumService)
        {
            _premiumService = premiumService;
        }

        // Pay a specific premium
        [HttpPost("premium/{premiumId}/pay")]
        public IActionResult PayPremium(Guid premiumId, [FromBody] PaymentDto paymentDto)
        {
            var result = _premiumService.PayPremium(premiumId, paymentDto);
            if (!result.Status)
                return BadRequest(result.Status);

            return Ok(result);
        }

        // Get all premiums and their statuses for a policy (for admin)
        [HttpGet("policy/{policyId}/premiums/status")]
        //[Authorize(Roles = "Admin")] // Restrict this endpoint to admins
        public IActionResult GetPremiumStatuses(Guid policyId)
        {
            var premiumStatuses = _premiumService.GetPremiumStatuses(policyId);
            return Ok(premiumStatuses);
        }
    }
}
