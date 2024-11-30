using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IPolicyService _policyService;
        private readonly IStateService _stateService;

        public AdminController(IAdminService adminService, IPolicyService policyService, IStateService stateService)
        {
            _adminService = adminService;
            _policyService = policyService;
            _stateService = stateService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var admins = _adminService.GetAll();
            return Ok(admins);
        }

        [HttpGet("Policy")]
        public IActionResult GetAllPolicies()
        {
            var policies = _policyService.GetAll();
            return Ok(policies);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var admin = _adminService.Get(id);
            return Ok(admin);
        }

        [HttpGet("Policy/{id}")]
        public IActionResult GetPolicy(Guid id)
        {
            var policy = _policyService.Get(id);
            return Ok(policy);
        }

        [HttpPost]
        public IActionResult Add(AdminRegisterDto admingisterDto)
        {
            var adminId = _adminService.Add(admingisterDto);
            return Ok(adminId);
        }

        [HttpPost("Policy")]
        public IActionResult AddPolicy(PolicyDto policyDto)
        {
            var policyId = _policyService.Add(policyDto);
            return Ok(policyId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_adminService.Delete(id))
                return Ok(id);
            return NotFound("Admin Not Found");
        }

        [HttpDelete("policy/{id}")]
        public IActionResult DeletePolicy(Guid id)
        {
            if (_policyService.Delete(id))
                return Ok(id);
            return NotFound("Policy not Found");
        }

        [HttpPut]
        public IActionResult Update(AdminDto adminDto)
        {
            if(_adminService.Update(adminDto))
                return Ok(adminDto);
            return NotFound("Admin not found");
        }

        [HttpPut("Policy")]
        public IActionResult UpdatePolicy(PolicyDto policyDto)
        {
            if (_policyService.Update(policyDto))
                return Ok(policyDto);
            return NotFound("Policy Not Found");
        }

        [HttpPost("State")]
        public IActionResult AddState(StateDto stateDto)
        {
            var state = _stateService.AddState(stateDto);
            return Ok(state);
        }
        [HttpPost("City")]
        public IActionResult AddCity(StateDto stateDto)
        {
            var city = _stateService.AddCity(stateDto);
            return Ok(city);
        }
        [HttpGet("States")]
        public IActionResult GetAllStates()
        {
            var states = _stateService.GetAllState();
            return Ok(states);
        }

    }
}
