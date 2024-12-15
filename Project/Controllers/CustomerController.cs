using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IPolicyService policyService, IPolicyAccountService policyAccountService, IMapper mapper)
        {
            _customerService = customerService;
            _policyService = policyService;
            _policyAccountService = policyAccountService;
            _mapper = mapper;
        }

        [HttpGet, Authorize(Roles = "CUSTOMER, AGENT")]
        public IActionResult GetAll([FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var customerDtos = _customerService.GetCustomers(pageParameter, ref count, searchQuery);
            return Ok(new {customerDtos= customerDtos, count=count});
        }

        [HttpPost, Authorize(Roles = "CUSTOMER")]
        public IActionResult Add(CustomerRegisterDto customerRegisterDto)
        {
            var id = _customerService.AddCustomer(customerRegisterDto);
            return Ok(id);
        }


        [HttpGet("{id}"), Authorize(Roles = "CUSTOMER")]
        public IActionResult Get(Guid id)
        {
            var customer = _customerService.GetById(id);
            return Ok(customer);
        }

        [HttpPut, Authorize(Roles = "CUSTOMER")]
        public IActionResult Update(UpdateCustomerDto customerDto)
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

        [HttpPut("ChangePassword"), Authorize(Roles = "CUSTOMER")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (_customerService.ChangePassword(changePasswordDto))
            {
                return Ok(changePasswordDto);
            }
            return NotFound("Agent not found");
        }

        [HttpPost("purchase-policy/{customerId}/Agent"), Authorize(Roles = "CUSTOMER")]
        public IActionResult PurchasePolicyAgent(Guid customerId, PurchasePolicyRequestDto requestdto)
        {
            Guid policyAccountId = new Guid();
            // 1. Validate inputs
            if (requestdto.PolicyId == Guid.Empty || requestdto.TotalAmount <= 0 || requestdto.DurationInYears <= 0)
            {
                return BadRequest(new {message = "Invalid purchase request." });
            }

            // 2. Link customer to policy and generate premiums
            var result = _policyService.PurchasePolicy(customerId, requestdto, ref policyAccountId);

            // 3. Return success or failure response
            if (result)
            {
                return Ok(new { accountId = policyAccountId });
            }
            return BadRequest(new {message = "Failed to purchase policy." });
        }

        [HttpPost("purchase-policy"), Authorize(Roles = "CUSTOMER")]
        public IActionResult PurchasePolicy(PurchasePolicyDto purchasedto)
        {
            var requestdto = _mapper.Map<PurchasePolicyRequestDto>(purchasedto);
            Guid policyAccountId = new Guid();
            // 1. Validate inputs
            if (requestdto.PolicyId == Guid.Empty || requestdto.TotalAmount <= 0 || requestdto.DurationInYears <= 0)
            {
                return BadRequest(new { message = "Invalid purchase request." });
            }
            
            // 2. Link customer to policy and generate premiums
            var result = _policyService.PurchasePolicy(purchasedto.CustomerId, requestdto, ref policyAccountId);

            // 3. Return success or failure response
            if (result)
            {
                return Ok(new { accountId = policyAccountId });
            }
            return BadRequest(new { message = "Failed to purchase policy." });
        }




        [HttpGet("Policy")]
        public IActionResult GetPolicyByCustomer(Guid id)
        {
            var policyDto = _policyService.GetPolicyByCustomer(id);
            return Ok(policyDto);
        }

        [HttpGet("PolicyAccount"), Authorize(Roles = "CUSTOMER")]
        public IActionResult GetPolicyAccountByCustomer(Guid id,[FromQuery] PageParameter pageParameter, [FromQuery] string? searchQuery)
        {
            var count = 0;
            var policyAccount = _policyAccountService.GetAccountByCustomer(id, pageParameter, ref count, searchQuery);
            return Ok(new {policyAccount = policyAccount, count = count});
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

        [HttpPut("Reupload/{id}"), Authorize(Roles = "CUSTOMER")]
        public IActionResult Reupload(Guid id)
        {
            if(_policyAccountService.ReUpload(id))
            {
                return Ok(id);
            }
            return BadRequest();
        }
        [HttpGet("GetInfo/{id}"), Authorize(Roles = "CUSTOMER")]
        public IActionResult GetInfo(Guid id)
        {

            var dto =_customerService.GetCustomerNameMobDto(id);
            return Ok(dto);
        }

    }
}
