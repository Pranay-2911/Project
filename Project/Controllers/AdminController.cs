using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminservice;

        public AdminController(IAdminService adminService)
        {
            _adminservice = adminService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var admins = _adminservice.GetAll();
            return Ok(admins);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var admin = _adminservice.Get(id);
            return Ok(admin);
        }

        [HttpPost]
        public IActionResult Add(AdminRegisterDto admingisterDto)
        {
            var adminId = _adminservice.Add(admingisterDto);
            return Ok(adminId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_adminservice.Delete(id))
                return Ok(id);
            return NotFound("Admin Not Found");
        }

        [HttpPut]
        public IActionResult Update(AdminDto adminDto)
        {
            if(_adminservice.Update(adminDto))
                return Ok(adminDto);
            return NotFound("Admin not found");
        }
    }
}
