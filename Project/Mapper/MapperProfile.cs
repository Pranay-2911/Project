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
            CreateMap<Employee, EmployeeRegisterDto>().ForMember(dest => dest.UserId, val => val.MapFrom(src => src.UserId));
            CreateMap<EmployeeRegisterDto, Employee>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.TotalDocuments, val => val.MapFrom(src => src.Documents.Count))
                .ForMember(dest => dest.TotalPolicies, val => val.MapFrom(src => src.Policies.Count));

            CreateMap<Customer, CustomerRegisterDto>().ForMember(dest => dest.UserId, val => val.MapFrom(src => src.UserId));
            CreateMap<CustomerRegisterDto, Customer>();

            CreateMap<CustomerDto, Customer>();

            CreateMap<Admin, AdminDto>()
                .ForMember(dest => dest.TotalEmployees, val => val.MapFrom(src => src.Employees.Count))
                .ForMember(dest => dest.TotalCustomers, val => val.MapFrom(src => src.Customers.Count))
                .ForMember(dest => dest.TotalAgents, val => val.MapFrom(src => src.Agents.Count))
                .ForMember(dest => dest.TotalPolicies, val => val.MapFrom(src => src.Policies.Count));
            CreateMap<AdminDto, Admin>();


            CreateMap<Agent, AgentDto>().ForMember(dest => dest.TotalCustomers, val => val.MapFrom(src => src.Customers.Count));
            CreateMap<AgentDto, Agent>();
            CreateMap<Agent, AgentRegisterDto>().ForMember(dest => dest.UserId, val => val.MapFrom(src => src.UserId));
            CreateMap<AgentRegisterDto, Agent>();

            CreateMap<PolicyAccountDto, PolicyAccount>();
            CreateMap<PolicyAccount, PolicyAccountDto>();
        }
    }
}
