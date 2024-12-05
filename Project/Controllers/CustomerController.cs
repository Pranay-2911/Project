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
        private readonly IPolicyService _policyService;
        private readonly IPolicyAccountService _policyAccountService;

        public CustomerController(ICustomerService customerService, IPolicyService policyService, IPolicyAccountService policyAccountService)
        {
            _customerService = customerService;
            _policyService = policyService;
            _policyAccountService = policyAccountService;
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
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (_customerService.ChangePassword(changePasswordDto))
            {
                return Ok(changePasswordDto);
            }
            return NotFound("Agent not found");
        }

        [HttpPost("customer/{customerId}/purchase-policy")]
        public IActionResult PurchasePolicy(Guid customerId, PurchasePolicyRequestDto requestdto)
        {
            // 1. Validate inputs
            if (requestdto.PolicyId == Guid.Empty || requestdto.TotalAmount <= 0 || requestdto.DurationInMonths <= 0)
            {
                return BadRequest(new {message = "Invalid purchase request." });
            }

            // 2. Link customer to policy and generate premiums
            var result = _policyService.PurchasePolicy(customerId, requestdto);

            // 3. Return success or failure response
            if (result)
            {
                return Ok(new {message = "Policy purchased successfully!" });
            }
            return BadRequest(new {message = "Failed to purchase policy." });
        }

       


        [HttpGet("Policy")]
        public IActionResult GetPolicyByCustomer(Guid id)
        {
            var policyDto = _policyService.GetPolicyByCustomer(id);
            return Ok(policyDto);
        }
        [HttpGet("PolicyAccount")]
        public IActionResult GetPolicyAccountByCustomer(Guid id)
        {
            var policyAccount = _policyAccountService.GetAccountByCustomer(id);
            return Ok(policyAccount);
        }

        [HttpDelete("{customerId}/{policyId}")]
        public IActionResult DeletePolicy(Guid customerId, Guid policyId)
        {
            if (_policyAccountService.Delete(customerId, policyId))
            {
                return Ok("Done");
            }
            return BadRequest();
        }
    }
}
