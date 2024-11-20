using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetRoles();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            var id = _userService.AddRole(user);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }
        [HttpPut]
        public IActionResult Update(User user)
        {
            if (_userService.UpdateRole(user))
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_userService.DeleteRole(id))
            {
                return Ok(id);
            }
            return NotFound();
        }
    }
}
