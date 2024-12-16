using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesCitiesController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StatesCitiesController(IStateService stateService)
        {
            _stateService = stateService;
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

        [HttpGet("States")]
        public IActionResult GetAllStates()
        {
            var states = _stateService.GetAllState();
            return Ok(states);
        }

        [HttpGet("City")]
        public IActionResult GetCity()
        {
            var city = _stateService.GetCities();
            return Ok(city);
        }
    }
}
