using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Models;
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

        [HttpGet, Authorize(Roles = "ADMIN, EMPLOYEE")]
        public IActionResult GetAll([FromQuery] PageParameter pageParameters, [FromQuery]string? searchQuery)
        {
            var count = 0;
            var agents = _agentService.GetAll(pageParameters, ref count, searchQuery);
            return Ok(new { agents = agents, count = count});
        }

        [HttpGet("UnVerified"), Authorize(Roles = "ADMIN")]
        public IActionResult GetAllUnVerified([FromQuery] PageParameter pageParameters, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var agents = _agentService.GetAllUnVerified(pageParameters, ref count, searchQuery);
            return Ok(new {agents = agents, count = count});
        }

        [HttpGet("{id}"), Authorize(Roles = "AGENT, EMPLOYEE")]
        public IActionResult Get(Guid id)
        {
            var agent = _agentService.Get(id);
            return Ok(agent);
        }

        [HttpPost, Authorize(Roles = "ADMIN, EMPLOYEE")]
        public IActionResult Add(AgentRegisterDto agentRegisterDto)
        {
            var agentId = _agentService.Add(agentRegisterDto);
            return Ok(agentId);
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Delete(Guid id)
        {
            if (_agentService.Delete(id))
                return Ok(id);
            return NotFound("Agent Not Found");
        }

        [HttpPut("Active/{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Active(Guid id)
        {
            if (_agentService.Active(id))
                return Ok(id);
            return NotFound("Agent Not Found");
        }

        [HttpPut("Verify"), Authorize(Roles = "ADMIN")]
        public IActionResult Verfify([FromQuery] Guid id)
        {
            if (_agentService.VerifyAgent(id))
                return Ok(new { message = "Verified Succesfully"});
            return NotFound("Agent not found");
        }

        [HttpPut, Authorize(Roles = "AGENT")]
        public IActionResult Update(UpdateAgentDto agentDto)
        {
            if (_agentService.Update(agentDto))
                return Ok(agentDto);
            return NotFound("Agent not found");
        }

        [HttpPut("ChangePassword"), Authorize(Roles = "AGENT")]
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


        [HttpGet("Commission/{id}"), Authorize(Roles = "ADMIN, AGENT, EMPLOYEE")]
        public IActionResult GetCommission(Guid id, [FromQuery] PageParameter pageParameters, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var viewCommissionDto = _policyService.GetCommissionByAgent(id, pageParameters, ref count, searchQuery);
            return Ok(new {viewCommissionDto = viewCommissionDto, count= count});
        }

        [HttpGet("CurrentBalance/{id}"), Authorize(Roles = "AGENT")]
        public IActionResult GetBalance(Guid id)
        {
            var balance = _agentService.GetBalance(id);
            return Ok(balance);
        }
        [HttpPost("CommissionRequest"), Authorize(Roles = "AGENT")]
        public IActionResult CommisionRequest(CommisionRequestDto commisionRequestDto)
        {
           var id  = _agentService.CommissionRequest(commisionRequestDto);
           return Ok(id);
        }

        [HttpGet("CommissionRequest/{id}"), Authorize(Roles = "AGENT")]
        public IActionResult GetCommissionRequest(Guid id, [FromQuery]PageParameter pageParameter, [FromQuery] string? selectedCommissionType)
        {
            var count = 0;
            var requests = _commissionRequestService.GetRequestByAgent(id, pageParameter, ref count, selectedCommissionType);
            return Ok(new {requests = requests, count = count});    
        }
    }
}
