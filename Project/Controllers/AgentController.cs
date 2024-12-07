using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;
        private readonly ICustomerService _customerService;
        private readonly IPolicyService _policyService;
        private readonly ICommissionRequestService _commissionRequestService;

        public AgentController(IAgentService agentService, ICustomerService customerService, IPolicyService policyService, ICommissionRequestService commissionRequestService)
        {
            _agentService = agentService;
            _customerService = customerService;
            _policyService = policyService;
            _commissionRequestService = commissionRequestService;

        }



        [HttpGet]
        public IActionResult GetAll()
        {
            var agents = _agentService.GetAll();
            return Ok(agents);
        }
        [HttpGet("UnVerified")]
        public IActionResult GetAllUnVerified()
        {
            var agents = _agentService.GetAllUnVerified();
            return Ok(agents);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var agent = _agentService.Get(id);
            return Ok(agent);
        }

        [HttpPost]
        public IActionResult Add(AgentRegisterDto agentRegisterDto)
        {
            var agentId = _agentService.Add(agentRegisterDto);
            return Ok(agentId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_agentService.Delete(id))
                return Ok(id);
            return NotFound("Agent Not Found");
        }
        [HttpPut("Verify")]
        public IActionResult Verfify([FromQuery] Guid id)
        {
            if (_agentService.VerifyAgent(id))
                return Ok(new { message = "Verified Succesfully"});
            return NotFound("Agent not found");
        }

        [HttpPut]
        public IActionResult Update(AgentDto agentDto)
        {
            if (_agentService.Update(agentDto))
                return Ok(agentDto);
            return NotFound("Agent not found");
        }

        [HttpPut("chnagepassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if(_agentService.ChangePassword(changePasswordDto))
            {
                return Ok(changePasswordDto);
            }
            return NotFound("Agent not found");


        }

        [HttpPost("customer")]
        public IActionResult AddCustomer(CustomerRegisterDto customerRegisterDto)
        {
            var customerId = _customerService.AddCustomer(customerRegisterDto);
            return Ok(customerId);
        }


        [HttpGet("Commission/{id}")]
        public IActionResult GetCommission(Guid id)
        {
            var viewCommissionDto = _policyService.GetCommissionByAgent(id);
            return Ok(viewCommissionDto);
        }
        [HttpGet("CurrentBalance/{id}")]
        public IActionResult GetBalance(Guid id)
        {
            var balance = _agentService.GetBalance(id);
            return Ok(balance);
        }
        [HttpPost("CommissionRequest")]
        public IActionResult CommisionRequest(CommisionRequestDto commisionRequestDto)
        {
           var id  = _agentService.CommissionRequest(commisionRequestDto);
           return Ok(id);
        }

        [HttpGet("CommissionRequest/{id}")]
        public IActionResult GetCommissionRequest(Guid id)
        {
            var requests = _commissionRequestService.GetRequestByAgent(id);
            return Ok(requests);    
        }
    }
}
