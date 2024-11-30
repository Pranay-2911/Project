using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        //[HttpPost]
        //public IActionResult Add(Policy policy)
        //{
        //    var newId = _policyService.Add(policy);
        //    return Ok(newId);
        //}

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
