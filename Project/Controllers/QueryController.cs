using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

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

        [HttpGet]
        public IActionResult Get()
        {
            var queryDto = _queryService.GetAllQuery();
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
