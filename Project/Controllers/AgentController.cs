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

        public AgentController(IAgentService agentService, ICustomerService customerService)
        {
            _agentService = agentService;
            _customerService = customerService;
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
    }
}
