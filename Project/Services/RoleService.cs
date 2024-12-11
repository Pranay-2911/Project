﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Mapper;
using Project.Models;
using Project.Repositories;
using Serilog;

namespace Project.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _repository;
        private readonly IMapper _mapper;
        public RoleService(IRepository<Role> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public Guid AddRole(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            _repository.Add(role);
            Log.Information("role record added: " + role.Id);

            return role.Id;
        }

        public bool DeleteRole(Guid id)
        {
            var role = _repository.Get(id);
            if (role != null)
            {
                _repository.Delete(role);
                Log.Information("role record deleted: " + role.Id);

                return true;
            }
            return false;

        }

        public Role GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public List<RoleDto> GetRoles()
        {
            var roles =  _repository.GetAll().Include(p => p.Users).ToList();
            List<RoleDto> roleDtos = _mapper.Map<List<RoleDto>>(roles);
            return roleDtos;
        }

        public bool UpdateRole(RoleDto roleDto)
        {
            var existingRole = _repository.GetAll().AsNoTracking().Where(r => r.Id == roleDto.Id);
            if (existingRole != null)
            {
                var role = _mapper.Map<Role>(roleDto);
                _repository.Update(role);
                Log.Information("role record updated: " + role.Id);

                return true;
            }
            return false;

        }
    }
}
