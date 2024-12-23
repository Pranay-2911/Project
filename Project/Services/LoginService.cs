﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Models;
using Project.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Services
{
    public class LoginService: ILoginService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Admin> _adminRepo;
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IRepository<Agent> _agentRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IConfiguration _configuration;

        public LoginService(IRepository<User> userRepo, IRepository<Admin> adminRepo, IRepository<Employee> employeeRepo,
            IRepository<Agent> agentRepo, IRepository<Customer> customerRepo, IConfiguration configuration)
        {
            _adminRepo = adminRepo;
            _userRepo = userRepo;
            _employeeRepo = employeeRepo;
            _agentRepo = agentRepo;
            _customerRepo = customerRepo;
            _configuration = configuration;
        }

        public User FindByUserName(string userName)
        {
            return _userRepo.GetAll().Include(u => u.Role).Where(u => u.UserName == userName).FirstOrDefault();       
        }

        public object FindUser(string role, Guid userId, ref string token, ref Guid id)
        {
            if (role == "ADMIN")
            {
               
                var admin = _adminRepo.GetAll().Where(u => u.UserId == userId).FirstOrDefault();
                token = CreateTokenAdmin(role, admin);
                id = admin.Id;
                return admin;
            }
            else if (role == "AGENT")
            {
                var agent = _agentRepo.GetAll().Where(u => u.UserId == userId).FirstOrDefault();
                token= CreateTokenAgent(role, agent);
                id = agent.Id;
                return agent;
            }
            else if (role == "CUSTOMER")
            {
                var customer =  _customerRepo.GetAll().Where(u => u.UserId == userId).FirstOrDefault();
                token = CreateTokenCustomer(role, customer);
                id = customer.CustomerId;
                return token;
            }
            else if (role == "EMPLOYEE")
            {
                var employee =  _employeeRepo.GetAll().Where(u => u.UserId == userId).FirstOrDefault();
                token = CreateTokenEmployee(role, employee);
                id = employee.Id;  
                return employee;
            }
            return null;
        }

        private string CreateTokenAdmin(string role,Admin user)
        {

            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //token construction
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            //generate the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string CreateTokenEmployee(string role, Employee user)
        {

            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Role, role),
                new Claim("Id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //token construction
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            //generate the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string CreateTokenAgent(string role, Agent user)
        {

            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Role, role),
                new Claim("Id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //token construction
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            //generate the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string CreateTokenCustomer(string role, Customer user)
        {

            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Role, role),
                new Claim("Id", user.CustomerId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //token construction
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            //generate the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }

}
