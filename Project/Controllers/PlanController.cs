using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        public PlanController(IPolicyService policyService) 
        {
            _policyService = policyService;
        }

        [HttpGet("Schema")]
        public IActionResult GetAllSchema()
        {
            var policyDto = _policyService.GetAllSchema();
            return Ok(policyDto);
        }
        [HttpGet]
        public IActionResult GetAllPlan()
        {
            var policyDto = _policyService.GetAllPlan();
            return Ok(policyDto);
        }

        [HttpPost("Schema")]
        public IActionResult AddSchema(PolicyDto policy)
        {
            var newId = _policyService.AddSchema(policy);
            return Ok(newId);
        }
        [HttpPost]
        public IActionResult AddPlan(PlanDto planDto)
        {
            var newId = _policyService.AddPlan(planDto);
            return Ok(newId);
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            if(_policyService.Delete(id))
            {
                return Ok(id);
            }
            return BadRequest();
        }
    }
}
