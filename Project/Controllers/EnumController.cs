using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        private readonly IEnumService _enumService;
        public EnumController(IEnumService enumService)
        {
            _enumService = enumService;
        }

        [HttpGet("Document")]
        public IActionResult GetDocumentType() 
        {
            var documentType = _enumService.GetDocument();
            return Ok(documentType);
        }
        [HttpGet("Commission")]
        public IActionResult GetCommissionType()
        {
            var commissionType = _enumService.GetCommisson();
            return Ok(commissionType);
        }
        [HttpGet("Nominee")]
        public IActionResult GetNomineeType()
        {
            var nomineeType = _enumService.GetNominee();
            return Ok(nomineeType);
        }

        [HttpGet("WithdrawStatus")]
        public IActionResult WithdrawStatus()
        {
            var withdrawStatus = _enumService.GetWithDrawStatus();
            return Ok(withdrawStatus);
        }
    }
}
