using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Mozaic.PasswordManager.Web.Models.DBEntities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public object Users { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
