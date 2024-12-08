using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;
using Project.Models;

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

        [HttpGet]
        public IActionResult GetAll([FromQuery] PageParameter pageParameter)
        {
            var employeeDtos = _employeeService.GetEmployees(pageParameter);
            return Ok(employeeDtos);
        }

        [HttpPost]
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
        [HttpPut]
        public IActionResult Update(EmployeeDto employeeDto)
        {
            if (_employeeService.UpdateEmployee(employeeDto))
            {
                return Ok(employeeDto);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_employeeService.DeleteEmployee(id))
            {
                return Ok(id);
            }
            return NotFound();
        }

        [HttpGet("GetAllDocument")]
        public IActionResult GetAllDocument([FromQuery] PageParameter pageParameters)
        {
            var documents = _employeeService.GetDocuments(pageParameters);
            return Ok(documents);
        }

        [HttpPut("Verify/{id}")]
        public IActionResult Verify(Guid id)
        {
            if (_employeeService.Verify(id))
            {
                return Ok(id);
            }
            return NotFound();
        }
        [HttpPut("Reject/{id}")]
        public IActionResult Reject(Guid id)
        {
            if (_employeeService.Reject(id))
            {
                return Ok(id);
            }
            return NotFound();
        }

        [HttpPut("QueryResponse/{id}")]
        public IActionResult Response(Guid id, QueryResponseDto queryResponseDto)
        {
            if (_queryService.Response(id, queryResponseDto))
            {
                return Ok(id);
            }
            return NotFound();
        }
        [HttpGet("GetAllQuery")]
        public IActionResult GetAllQuery([FromQuery]PageParameter pageParameter)
        {
            var queries = _queryService.GetAllQuery(pageParameter);
            return Ok(queries);
        }
        [HttpPut("ChangePassword")]
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
