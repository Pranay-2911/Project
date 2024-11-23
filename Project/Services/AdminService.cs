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

        public AdminService(IRepository<Admin> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public Guid Add(AdminDto adminDto)
        {
            var admin = _mapper.Map<Admin>(adminDto);
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
