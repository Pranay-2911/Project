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
        private readonly IVariableService _globalVariableService;

        public AdminController(IAdminService adminService, IPolicyService policyService,  IVariableService globalVariableService)
        {
            _adminService = adminService;
            _policyService = policyService;
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

      

        [HttpGet("Commission"), Authorize(Roles = "ADMIN")]
        public IActionResult GetCommission([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery, [FromQuery]string? selectedCommissionType, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        { 
            var count = 0;
            var viewCommissionDto = _policyService.GetCommission(pageParameter, ref count, searchQuery, selectedCommissionType, startDate, endDate);
            return Ok(new { viewCommissionDto = viewCommissionDto, count = count});
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


    }
}
