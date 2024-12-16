using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;
using Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IQueryService _queryService;
        public EmployeeController(IEmployeeService employeeService, IQueryService queryService)
        {
            _employeeService = employeeService;
            _queryService = queryService;
        }

        [HttpGet, Authorize(Roles = "ADMIN")]
        public IActionResult GetAll([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var employeeDtos = _employeeService.GetEmployees(pageParameter, ref count, searchQuery);
            return Ok(new {employeeDtos = employeeDtos, count = count});
        }

        [HttpPost, Authorize(Roles = "ADMIN")]
        public IActionResult Add(EmployeeRegisterDto employeeRegisterDto)
        {
            var id = _employeeService.AddEmployee(employeeRegisterDto);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var employee = _employeeService.GetById(id);
            return Ok(employee);
        }
        [HttpPut, Authorize(Roles = "EMPLOYEE")]
        public IActionResult Update(UpdateEmployeeDto employeeDto)
        {
            if (_employeeService.UpdateEmployee(employeeDto))
            {
                return Ok(employeeDto);
            }
            return NotFound();
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Delete(Guid id)
        {
            if (_employeeService.DeleteEmployee(id))
            {
                return Ok(id);
            }
            return NotFound();
        }

        [HttpPut("Active/{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult Active(Guid id)
        {
            if (_employeeService.Active(id))
            {
                return Ok(id);
            }
            return NotFound();
        }

        [HttpGet("GetAllDocument"), Authorize(Roles = "EMPLOYEE")]
        public IActionResult GetAllDocument([FromQuery] PageParameter pageParameters, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var documents = _employeeService.GetDocuments(pageParameters, ref count, searchQuery);
            return Ok(new {documents= documents, count = count});
        }

        [HttpPut("Verify/{id}"), Authorize(Roles = "EMPLOYEE")]
        public IActionResult Verify(Guid id)
        {
            if (_employeeService.Verify(id))
            {
                return Ok(id);
            }
            return NotFound();
        }
        [HttpPut("Reject/{id}"), Authorize(Roles = "EMPLOYEE")]
        public IActionResult Reject(Guid id)
        {
            if (_employeeService.Reject(id))
            {
                return Ok(id);
            }
            return NotFound();
        }

        [HttpPut("QueryResponse/{id}"), Authorize(Roles = "EMPLOYEE")]
        public IActionResult Response(Guid id, QueryResponseDto queryResponseDto)
        {
            if (_queryService.Response(id, queryResponseDto))
            {
                return Ok(id);
            }
            return NotFound();
        }
        [HttpGet("GetAllQuery"), Authorize(Roles = "EMPLOYEE")]
        public IActionResult GetAllQuery([FromQuery]PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var queries = _queryService.GetAllQuery(pageParameter, ref count, searchQuery);
            return Ok(new { queries = queries, count = count});
        }
        [HttpPut("ChangePassword"), Authorize(Roles = "EMPLOYEE")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (_employeeService.ChangePassword(changePasswordDto))
            {
                return Ok(changePasswordDto);
            }
            return NotFound("Agent not found");
        }

        //[HttpPost]
        //public IActionResult AddAgent(AgentDto agentDto)
        //{

        //}
    }
}
