﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;
using Project.Types;

namespace Project.Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository<Agent> _agentRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private Guid _roleId = new Guid("bd9ae755-dced-4820-931e-08dd0ecdec34");

        public AgentService(IRepository<Agent> repository, IMapper mapper, IRepository<Role> roleRepository, IRepository<User> userRepository)
        {
            _agentRepository = repository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public Guid Add(AgentRegisterDto agentRegisterDto)
        {
            User user = _mapper.Map<User>(agentRegisterDto);
            user.RoleId = _roleId;
            user.Status = true;
            _userRepository.Add(user);

            var role = _roleRepository.Get(_roleId);
            role.Users.Add(user);
            _roleRepository.Update(role);

            agentRegisterDto.UserId = user.Id;

            var agent = _mapper.Map<Agent>(agentRegisterDto);
            _agentRepository.Add(agent);
            return agent.Id;
        }

        public bool ChangePassword(ChnagePasswordDto passwordDto)
        {
            var agent = _agentRepository.GetAll().AsNoTracking().Include(a => a.User).Where(a => a.User.UserName == passwordDto.UserName && a.User.PasswordHash == passwordDto.Password).FirstOrDefault();
            if (agent != null)
            {
                agent.User.PasswordHash = passwordDto.NewPassword;
                _agentRepository.Update(agent);
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                _agentRepository.Delete(agent);
                return true;
            }
            return false;
        }

        public AgentDto Get(Guid id)
        {
            var agent = _agentRepository.Get(id);
            if (agent != null)
            {
                var agentDto = _mapper.Map<AgentDto>(agent);
                return agentDto;
            }
            throw new Exception("No such agent exist");
        }

        public List<Agent> GetAll()
        {
            var agents = _agentRepository.GetAll().Include(a => a.User).ToList();
            //var agentDtos = _mapper.Map<List<AgentDto>>(agents);
            return agents;
        }

        public bool Update(AgentDto agentDto)
        {
            var existingAgent = _agentRepository.GetAll().AsNoTracking().Where(u => u.Id == agentDto.Id);
            if (existingAgent != null)
            {
                var agent = _mapper.Map<Agent>(agentDto);
                _agentRepository.Update(agent);
                return true;
            }
            return false;
        }
    }
}
