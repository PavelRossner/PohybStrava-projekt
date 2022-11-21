using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using PohybStrava.Models;

namespace PohybStrava.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PohybStrava.Models.Activities> Activities { get; set; }
        public DbSet<PohybStrava.Models.Diet> Diet { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<PrijemVydejEnergie> PrijemVydejEnergie { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}