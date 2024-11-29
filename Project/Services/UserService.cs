﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        public UserService(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Guid AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _repository.Add(user);
            return user.Id;
        }

        public bool DeleteUser(Guid id)
        {
            var user = _repository.Get(id);
            if (user != null)
            {
                _repository.Delete(user);
                return true;
            }
            return false;

        }

        public User GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public List<UserDto> GetUsers()
        {
            var user = _repository.GetAll().ToList();
            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(user);
            return userDtos;
        }

        public bool UpdateUser(UserDto userDto)
        {
            var existingUser = _repository.GetAll().AsNoTracking().Where(u => u.Id == userDto.Id);
            if (existingUser != null)
            {
                var user = _mapper.Map<User>(userDto);
                _repository.Update(user);
                return true;
            }
            return false;

        }

        public User FindUserByName(string userName)
        {
            return _repository.GetAll().Include(u => u.Role).Where(u => u.UserName == userName).FirstOrDefault();
        }
    }
}
