using eProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace eProject.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
        { }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    IsActive = true
                },
                new Role
                {
                    Id = 2,
                    Name = "User",
                    IsActive = true
                }
            );
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Trịnh Xuân Vinh Quy",
                    Phone = "0946453657",
                    Email = "txvq0101@gmail.com",
                    UserName = "Admin",
                    Password = "Quy@0104",
                    Address = "451 Tôn Đản",
                    Province = 1,
                    District = 1,
                    Town = 1,
                    RoleId = 1,
                    IsActive = true,
                },
                new User
                {
                    Id = 2,
                    Name = "Trịnh Xuân Vinh Quy",
                    Phone = "0946453657",
                    Email = "beobubam1807@gmail.com",
                    UserName = "txvQuy",
                    Password = "Quy@0104",
                    Address = "451 Tôn Đản",
                    Province = 1,
                    District = 1,
                    Town = 1,
                    RoleId = 2,
                    IsActive = true,
                }
            );
        }
    }
}
