
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Data;
using Project.Exceptions;
using Project.Mapper;
using Project.Repositories;
using Project.Services;
using Stripe;
using System.Text;
using System.Text.Json.Serialization;
using Serilog;


namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<ProjectContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnString"));
            });

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IEmployeeService, EmployeeService>();
            builder.Services.AddTransient<ICustomerService, Services.CustomerService>();
            builder.Services.AddTransient<IAgentService, AgentService>();  
            builder.Services.AddTransient<IAdminService, AdminService>();
            builder.Services.AddTransient<ILoginService, LoginService>();
            builder.Services.AddTransient<IPolicyService, PolicyService>();
            builder.Services.AddTransient<IStateService, StateService>();
            builder.Services.AddTransient<IPremiumService, PremiumService>();
            builder.Services.AddTransient<IPolicyAccountService,  PolicyAccountService>();
            builder.Services.AddTransient<IEnumService, EnumService>();
            builder.Services.AddTransient<IDocumentService,  DocumentService>();
            builder.Services.AddTransient<IQueryService, QueryService>();
            builder.Services.AddTransient<ICommissionRequestService, CommissionRequestService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<IVariableService, VariableService>();
            builder.Services.AddTransient<IForgetPasswordService, ForgetPasswordService>();



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("*");
                });
            });



            //add auth scheme--validating token
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(builder.Configuration.GetSection("AppSettings:Key").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddExceptionHandler<AppExceptionHandler>();
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .WriteTo.File("Logs/app-log-.txt", rollingInterval: RollingInterval.Day,
              rollOnFileSizeLimit: true,            // Creates a new log file when size limit is reached
              shared: true) // Logs to a file
              .CreateLogger();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(_ => { });

            app.UseHttpsRedirection();
            app.UseCors("AllowAngularApp");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
