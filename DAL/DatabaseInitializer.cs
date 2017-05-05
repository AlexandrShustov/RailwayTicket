using System;
using System.Data.Entity;
using DAL;
using Domain.Entities;
using Microsoft.AspNet.Identity;

namespace DAL
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var adminUser = new User
            {
                Email = "admin@admin.com",
                FirstName = "admin",
                LastName = "admin",
                IsStudent = false,
                PhoneNumber = "+380999999997",
                UserName = "admin@admin.com",
                PasswordHash = new PasswordHasher().HashPassword("123456"),
                SecurityStamp = "ecbb5e4e-137b-4a62-b956-2d43ff2ff236",
                UserId = Guid.NewGuid()
            };

            var moderUser = new User
            {
                Email = "moder@moder.com",
                FirstName = "moder",
                LastName = "moder",
                IsStudent = false,
                PhoneNumber = "+380999999998",
                UserName = "moder@moder.com",
                PasswordHash = new PasswordHasher().HashPassword("123456"),
                SecurityStamp = "eccb5e4e-137b-4a62-b956-2d43ff2ff236",
                UserId = Guid.NewGuid()
            };

            var commonUser = new User
            {
                Email = "user@user.com",
                FirstName = "user",
                LastName = "user",
                IsStudent = false,
                PhoneNumber = "+380999999999",
                UserName = "user@user.com",
                PasswordHash = new PasswordHasher().HashPassword("123456"),
                SecurityStamp = "ezzb5e4e-137b-4a62-b956-2d43ff2ff236",
                UserId = Guid.NewGuid()
            };

            var adminRole = new Role{ Name = "admin", RoleId = Guid.NewGuid()};
            var moderRole = new Role { Name = "moder", RoleId = Guid.NewGuid() };
            var userRole = new Role { Name = "user", RoleId = Guid.NewGuid() };
            adminRole.Users.Add(adminUser);
            moderRole.Users.Add(moderUser);
            userRole.Users.Add(commonUser);

            context.Set<User>().Add(adminUser);
            context.Set<User>().Add(moderUser);
            context.Set<User>().Add(commonUser);

            context.Set<Role>().Add(adminRole);
            context.Set<Role>().Add(moderRole);
            context.Set<Role>().Add(userRole);

            base.Seed(context);
        }
    }
}