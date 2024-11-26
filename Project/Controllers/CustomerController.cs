using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customerDtos = _customerService.GetCustomers();
            return Ok(customerDtos);
        }

        [HttpPost]
        public IActionResult Add(CustomerRegisterDto customerRegisterDto)
        {
            var id = _customerService.AddCustomer(customerRegisterDto);
            return Ok(id);
        }
        [HttpPost("PolicyAccount")]
        public IActionResult AddPolicyAccount(PolicyAccountDto policyAccountDto)
        {
            var id = 
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var customer = _customerService.GetById(id);
            return Ok(customer);
        }
        [HttpPut]
        public IActionResult Update(CustomerDto customerDto)
        {
            if (_customerService.UpdateCustomer(customerDto))
            {
                return Ok(customerDto);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_customerService.DeleteCustomer(id))
            {
                return Ok(id);
            }
            return NotFound();
        }

        [HttpPut("chnagepassword")]
        public IActionResult ChangePassword(ChnagePasswordDto chnagePasswordDto)
        {
            if (_customerService.ChangePassword(chnagePasswordDto))
            {
                return Ok(chnagePasswordDto);
            }
            return NotFound("Agent not found");


        }
    }
}
