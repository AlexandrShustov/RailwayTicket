using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using BLL.Concrete;
using DAL;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using IdentityRole = WebUI.Identity.IdentityRole;
using IdentityUser = WebUI.Identity.IdentityUser;

namespace WebUI.Infrastructure
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        [Inject]
        public IUserStore<IdentityUser, Guid> _userStore { get; set; }

        protected override async void Seed(ApplicationDbContext context)
        {
            var userManager = DependencyResolver.Current.GetService<UserManager<IdentityUser, Guid>>();
            var userStor = DependencyResolver.Current.GetService<IUserService>();
 
            var roleManager = DependencyResolver.Current.GetService<RoleManager<IdentityRole, Guid>>();

            var adminRole = new IdentityRole("admin");
            var moderRole = new IdentityRole("moder");
            var userRole = new IdentityRole("user");

            //await roleManager.CreateAsync(adminRole);
            //await roleManager.CreateAsync(moderRole);
            //await roleManager.CreateAsync(userRole);

            roleManager.Create(adminRole);
            roleManager.Create(moderRole);
            roleManager.Create(userRole);

            var admin = new IdentityUser("admin@admin.com");
            var moder = new IdentityUser("moder@moder.com");
            var user = new IdentityUser("user@user.com");

            await userManager.CreateAsync(admin, "123456");
            await userManager.AddToRoleAsync(admin.Id, adminRole.Name);

            var entityUser = new User
            {
                Email = admin.UserName,
                FirstName = "admin",
                LastName = "admin",
                IsStudent = false,
                PhoneNumber = "+380957500085",
                UserName = admin.UserName,
                PasswordHash = admin.PasswordHash,
                SecurityStamp = admin.SecurityStamp,
                UserId = admin.Id
            };
            await userStor.UpdateAsync(entityUser);
            //await userManager.CreateAsync(moder, "123456");
            //await userManager.CreateAsync(user, "123456");

            //await userManager.AddToRoleAsync(moder.Id, "moder");
            //await userManager.AddToRoleAsync(user.Id, "user");

            base.Seed(context);
        }


    }
}