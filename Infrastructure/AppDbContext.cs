
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) :
            base(options)
        {
            HttpContext = contextAccessor.HttpContext;
        }
        public HttpContext? HttpContext { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}