using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("Payments"), Authorize(Roles = "ADMIN")]
        public IActionResult GetAllPayments([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var count = 0;
            var payments = _paymentService.GetAll(pageParameter, ref count, searchQuery, startDate, endDate);
            return Ok(new { payments = payments, count = count });
        }
    }
}
