using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Concrete;
using DAL;
using Microsoft.AspNet.Identity;
using WebUI.Identity;

namespace WebUI.Infrastructure
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override async void Seed(ApplicationDbContext context)
        {
            var mapper = new Mapper(new MapperConfiguration(Mapper => Mapper.AddProfile(new MapperProfile())));

            var unitOfWork = new UnitOfWork("RailwayTickets");
            var userService = new UserService(unitOfWork);
            var userManager = new UserManager<IdentityUser, Guid>(new UserStore(userService, mapper));

            var roleService = new RoleService(unitOfWork);
            var roleManager = new RoleManager<IdentityRole, Guid>(new RoleStore(roleService, mapper));

            var adminRole = new IdentityRole("admin", Guid.NewGuid());
            var moderRole = new IdentityRole("moder", Guid.NewGuid());
            var userRole = new IdentityRole("user", Guid.NewGuid());

            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(moderRole);
            await roleManager.CreateAsync(userRole);

            var admin = new IdentityUser("admin@admin.com");
            var moder = new IdentityUser("moder@moder.com");
            var user = new IdentityUser("user@user.com");

            userManager.Create(admin, "123456");
            userManager.Create(moder, "123456");
            userManager.Create(user, "123456");

            userManager.AddToRole(admin.Id, "admin");
            userManager.AddToRole(moder.Id, "moder");
            userManager.AddToRole(user.Id, "user");

            base.Seed(context);
        }
    }
}