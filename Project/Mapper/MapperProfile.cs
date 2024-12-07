using AutoMapper;
using Project.DTOs;
using Project.Models;

namespace Project.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Role, RoleDto>().ForMember(dest => dest.TotalUser, val => val.MapFrom(src => src.Users.Count));
            CreateMap<RoleDto, Role>(); 
            CreateMap<User, UserDto>();  
            CreateMap<UserDto, User>();
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeDto, Employee>();
            CreateMap<Employee, EmployeeRegisterDto>();
            CreateMap<EmployeeRegisterDto, Employee>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.TotalPolicies, val => val.MapFrom(src => src.Accounts.Count));

            CreateMap<Customer, CustomerRegisterDto>();
            CreateMap<CustomerRegisterDto, Customer>();

            CreateMap<CustomerDto, Customer>();

            CreateMap<Admin, AdminDto>();
            CreateMap<AdminDto, Admin>();


            CreateMap<Agent, AgentDto>();
            CreateMap<AgentDto, Agent>();
            CreateMap<Agent, AgentRegisterDto>();
            CreateMap<AgentRegisterDto, Agent>();

            //CreateMap<PolicyAccountDto, PolicyAccount>();
            //CreateMap<PolicyAccount, PolicyAccountDto>();

            CreateMap<AdminRegisterDto, User>().ForMember(dest => dest.PasswordHash, val => val.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
            CreateMap<AgentRegisterDto, User>().ForMember(dest => dest.PasswordHash, val => val.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
            CreateMap<EmployeeRegisterDto, User>().ForMember(dest => dest.PasswordHash, val => val.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
            CreateMap<CustomerRegisterDto, User>().ForMember(dest => dest.PasswordHash, val => val.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

            CreateMap<AdminRegisterDto, Admin>().ForMember(dest => dest.UserId, val => val.MapFrom(src => src.UserId));
            CreateMap<Admin, AdminRegisterDto>();

            CreateMap<Policy, PolicyDto>()
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => MapperHelper.SplitDocuments(src.Documents)));
            CreateMap<PolicyDto, Policy>().ForMember(dest => dest.Documents, val=>val.MapFrom(src=> string.Join(",", src.Documents)));

            CreateMap<Plan, PlanDto>();
            CreateMap<PlanDto, Plan>(); 

            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentDto, Document>();

            CreateMap<Premium, PremiumDto>();
            CreateMap<PremiumDto, Premium>();

            CreateMap<PurchasePolicyRequestDto, PurchasePolicyDto>();
            CreateMap<PurchasePolicyDto, PurchasePolicyRequestDto>();

            CreateMap<Query, AddQueryDto>();
            CreateMap<AddQueryDto, Query>();

            CreateMap<Query, ViewQueryDto>();
            CreateMap<ViewQueryDto, Query>();

            CreateMap<CommissionRequest, CommisionRequestDto>();
            CreateMap<CommisionRequestDto, CommissionRequest>();
        }
    }
}
