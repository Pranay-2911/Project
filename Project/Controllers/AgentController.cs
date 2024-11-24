﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var agents = _agentService.GetAll();
            return Ok(agents);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var agent = _agentService.Get(id);
            return Ok(agent);
        }

        [HttpPost]
        public IActionResult Add(AgentDto agentDto)
        {
            var agentId = _agentService.Add(agentDto);
            return Ok(agentId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_agentService.Delete(id))
                return Ok(id);
            return NotFound("Agent Not Found");
        }

        [HttpPut]
        public IActionResult Update(AgentDto agentDto)
        {
            if (_agentService.Update(agentDto))
                return Ok(agentDto);
            return NotFound("Agent not found");
        }

        [HttpPut("chnagepassword")]
        public IActionResult ChangePassword(ChnagePasswordDto chnagePasswordDto)
        {
            if(_agentService.ChangePassword(chnagePasswordDto))
            {
                return Ok(chnagePasswordDto);
            }
            return NotFound("Agent not found");


        }
    }
}
