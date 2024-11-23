using AutoMapper;
using Project.DTOs;
using Project.Models;

namespace Project.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Role, RoleDto>();//.ForMember(dest => dest.RoleName, val => val.MapFrom(src => src.RoleName));
            CreateMap<RoleDto, Role>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Employee, EmployeeDto>()
                     .ForMember(dest => dest.TotalCustomers, val => val.MapFrom(src => src.Customers.Count))
                     .ForMember(dest => dest.TotalAgents, val => val.MapFrom(src => src.Agents.Count));

            CreateMap<EmployeeDto, Employee>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.TotalDocuments, val => val.MapFrom(src => src.Documents.Count))
                .ForMember(dest => dest.TotalPolicies, val => val.MapFrom(src => src.Policies.Count));

            CreateMap<CustomerDto, Customer>();
            CreateMap<AdminDto, Admin>();
            CreateMap<Admin, AdminDto>();
            CreateMap<AgentDto, Agent>();
            CreateMap<Agent, AgentDto>();
        }
    }
}
