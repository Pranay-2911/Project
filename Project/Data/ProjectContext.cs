using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ProjectContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Customer> Customers { get; set; }  
        public DbSet<Document> Documents { get; set; }
        public DbSet<Employee> Employees { get; set; }  
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyAccount> PolicyAccounts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Premium> Premiums { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<GlobalVariables> GlobalVariables { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<CommissionRequest> CommissionRequests { get; set; }
        public DbSet<EmailRequest> EmailRequests { get; set; }



       
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }


    }
}
