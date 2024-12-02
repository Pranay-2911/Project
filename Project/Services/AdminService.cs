using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DTOs;
using Project.Exceptions;
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
        private readonly IRepository<City> _cityRepository;

        public AdminService(IRepository<Admin> repository, IMapper mapper, IRepository<User> userRepository, IRepository<Role> roleRepository, IRepository<State> stateRepository, IRepository<City> cityRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }
        public Guid Add(AdminRegisterDto adminRgisterDto)
        {
            var roleName = _roleRepository.GetAll().Where(r => r.RoleName == Types.Roles.ADMIN).FirstOrDefault();
            User user = _mapper.Map<User>(adminRgisterDto);
            user.RoleId = roleName.Id;
            user.Status = true;
            _userRepository.Add(user);

            var role = _roleRepository.Get(roleName.Id);
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
            throw new AdminNotFoundException("Admin Does Not Exist");
        }

        public AdminDto Get(Guid id)
        {
            var admin = _repository.Get(id);
            if (admin != null)
            {
                var adminDto = _mapper.Map<AdminDto>(admin);
                return adminDto;
            }
            throw new AdminNotFoundException("Admin Does Not Exist");
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
            throw new AdminNotFoundException("Admin Does Not Exist");
        }
        

    }
}
