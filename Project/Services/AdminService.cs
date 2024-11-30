using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Models;
using Project.Repositories;

namespace Project.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _repository;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<State> _stateRepository;
        private Guid _roleId = new Guid("daabeb97-b5d8-476a-931d-08dd0ecdec34");

        public AdminService(IRepository<Admin> repository, IMapper mapper, IRepository<User> userRepository, IRepository<Role> roleRepository, IRepository<State> stateRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _stateRepository = stateRepository;
        }
        public Guid Add(AdminRegisterDto adminRgisterDto)
        {
            User user = _mapper.Map<User>(adminRgisterDto);
            user.RoleId = _roleId;
            user.Status = true;
            _userRepository.Add(user);

            var role = _roleRepository.Get(_roleId);
            role.Users.Add(user);
            _roleRepository.Update(role);

            adminRgisterDto.UserId = user.Id;
            
            var admin = _mapper.Map<Admin>(adminRgisterDto);
            _repository.Add(admin);
            return admin.Id;
        }

        public bool Delete(Guid id)
        {
            var admin = _repository.Get(id);
            if (admin != null)
            {
                _repository.Delete(admin);
                return true;
            }
            return false;
        }

        public AdminDto Get(Guid id)
        {
            var admin = _repository.Get(id);
            if (admin != null)
            {
                var adminDto = _mapper.Map<AdminDto>(admin);
                return adminDto;
            }
            throw new Exception("No such admin exist");
        }

        public List<AdminDto> GetAll()
        {
            var admins = _repository.GetAll();
            var adminDtos = _mapper.Map<List<AdminDto>>(admins);
            return adminDtos;
        }

        public bool Update(AdminDto adminDto)
        {
            var existingAdmin = _repository.GetAll().AsNoTracking().Where(u => u.Id == adminDto.Id);
            if (existingAdmin != null)
            {
                var admin = _mapper.Map<Admin>(adminDto);
                _repository.Update(admin);
                return true;
            }
            return false;
        }
    }
}
