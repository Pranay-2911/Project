using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IQueryService _queryService;
        public QueryController(IQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id, [FromQuery]PageParameter pageParameter)
        {
            var queryDto = _queryService.GetQueryByCustomer(id, pageParameter);
            return Ok(queryDto);

        }

        [HttpPost]
        public IActionResult AddQuery(AddQueryDto addQueryDto)
        {
            var id = _queryService.AddQuery(addQueryDto);
            return Ok(id);
        }
    }
}
