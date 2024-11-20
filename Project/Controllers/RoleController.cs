﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleService.GetRoles();
            return Ok(roles);
        }

        [HttpPost]
        public IActionResult Add(Role role)
        {
            var id = _roleService.AddRole(role);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var role = _roleService.GetById(id);
            return Ok(role);
        }
        [HttpPut]
        public IActionResult Update(Role role)
        {
            if (_roleService.UpdateRole(role))
            {
                return Ok(role);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_roleService.DeleteRole(id))
            {
                return Ok(id);
            }
            return NotFound();
        }
    }
}