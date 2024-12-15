using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
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
        private readonly IPolicyAccountService _policyAccountService;
        private readonly ICommissionRequestService _commissionRequestService;
        private readonly IVariableService _globalVariableService;
        private readonly IPaymentService _paymentService;

        public AdminController(IAdminService adminService, IPolicyService policyService, IStateService stateService, IPolicyAccountService policyAccountService, ICommissionRequestService commissionRequestService, IPaymentService paymentService, IVariableService globalVariableService)
        {
            _adminService = adminService;
            _policyService = policyService;
            _stateService = stateService;
            _policyAccountService = policyAccountService;
            _commissionRequestService = commissionRequestService;
            _paymentService = paymentService;
            _globalVariableService = globalVariableService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var admins = _adminService.GetAll();
            return Ok(admins);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var admin = _adminService.Get(id);
            return Ok(admin);
        }

        [HttpPost]
        public IActionResult Add(AdminRegisterDto admingisterDto)
        {
            var adminId = _adminService.Add(admingisterDto);
            return Ok(adminId);
        }

       


        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_adminService.Delete(id))
                return Ok(id);
            return NotFound("Admin Not Found");
        }       

        [HttpPut]
        public IActionResult Update(AdminDto adminDto)
        {
            if(_adminService.Update(adminDto))
                return Ok(adminDto);
            return NotFound("Admin not found");
        }

        [HttpPost("State"), Authorize(Roles = "ADMIN")]
        public IActionResult AddState(StateDto stateDto)
        {
            var state = _stateService.AddState(stateDto);
            return Ok(state);
        }

        [HttpPost("City"), Authorize(Roles = "ADMIN")]
        public IActionResult AddCity(StateDto stateDto)
        {
            var city = _stateService.AddCity(stateDto);
            return Ok(city);
        }

        [HttpGet("States" ), Authorize(Roles = "ADMIN,CUSTOMER")]
        public IActionResult GetAllStates()
        {
            var states = _stateService.GetAllState();
            return Ok(states);
        }

        [HttpGet("City"), Authorize(Roles = "ADMIN")]
        public IActionResult GetCity()
        {
            var city = _stateService.GetCities();
            return Ok(city);
        }   

        [HttpGet("PolicyAccount"), Authorize(Roles = "ADMIN")]
        public IActionResult GetPolicyAccount([FromQuery]PageParameter pageParameter, [FromQuery] string? searchQuery, [FromQuery]string? searchQuery1)
        {
            var count = 0;
            var accounts = _policyAccountService.GetAll(pageParameter, ref count, searchQuery, searchQuery1);
            return Ok(new {accounts =  accounts, count = count});
        }

        [HttpGet("Commission"), Authorize(Roles = "ADMIN")]
        public IActionResult GetCommission([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery, [FromQuery]string? selectedCommissionType, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        { 
            var count = 0;
            var viewCommissionDto = _policyService.GetCommission(pageParameter, ref count, searchQuery, selectedCommissionType, startDate, endDate);
            return Ok(new { viewCommissionDto = viewCommissionDto, count = count});
        }

        [HttpGet("CommissionRequest"), Authorize(Roles = "ADMIN")]
        public IActionResult GetAllRequest([FromQuery]PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var requests = _commissionRequestService.GetAllPendingRequest(pageParameter, ref count, searchQuery);
            return Ok(new { requests = requests, count = count});
        }
        [HttpPut("CommissionRequest/Approve/{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Approve(Guid id)
        {
           if(_commissionRequestService.Approve(id))
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

        [HttpPut("Global"), Authorize(Roles = "ADMIN")]
        public IActionResult UpdateGlobal(GlobalVariables globalVariables)
        {
            if (_globalVariableService.UpdateGlobal(globalVariables))
            {
                return Ok(new { message = "Successfully Updated"});
            }
            return BadRequest();
        }
        [HttpGet("Global"), Authorize(Roles = "ADMIN")]
        public IActionResult GetGlobal()
        {
            var global = _globalVariableService.Get();
            return Ok(global);
        }
        [HttpPost("Global"), Authorize(Roles = "ADMIN")]
        public IActionResult AddGlobal(GlobalVariables globalVariables)
        {
            _globalVariableService.AddGlobal(globalVariables);
            return Ok();    
        }

        [HttpGet("Payments"), Authorize(Roles = "ADMIN")]
        public IActionResult GetAllPayments([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery, [FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate) 
        {
            var count = 0;
            var payments = _paymentService.GetAll(pageParameter, ref count, searchQuery, startDate, endDate);
            return Ok(new {payments = payments, count= count});
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
           if(_policyAccountService.ApproveClaims(id))
           {
                return Ok(id);
           }
           return BadRequest(); 
            
        }

    }
}
