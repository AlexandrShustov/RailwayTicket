using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using DAL;
using Domain.Entities;
using Domain.Enumerations;
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



            var places = new List<Place>();
            var carriages = new List<Carriage>();
            var trainsList = new List<Train>();
            var stationsList = new List<Station>();
            var routeStationsList = new List<RouteStation>();
            var routesList = new List<Route>();

            for (int i = 0; i < 40; i++)
            {
                var place = new Place { IsFree = true, Number = i+1 };
                places.Add(place);
            }

            places.ElementAt(4).IsFree = false;
            places.ElementAt(5).IsFree = false;

            var carriage = new Carriage{ Number = 1, Places = places };
            carriage.CarriageType = CarriageType.Compartments;
            carriage.IsDeleted = false;
            carriages.Add(carriage);
            
            var train = new Train{ Carriages = carriages, Number = 444 };
            trainsList.Add(train);
            
            var route = new Route();
            route.Train = train;
     
            var stationOne = new Station {Name = "Kharkov"};
            var stationTwo = new Station { Name = "Kiev" };
            stationOne.IsDeleted = false;
            stationOne.IsDeleted = false;
            stationsList.Add(stationOne);
            stationsList.Add(stationTwo);

            var routeStationStart = new RouteStation{ ArriveTime = new DateTime(2017, 5, 15, 23, 23, 0),
                                                      DepartureTime = new DateTime(2017, 5, 16, 1, 15, 0), Station = stationOne};
            var routeStationEnd = new RouteStation { ArriveTime = new DateTime(2017, 5, 16, 2, 30, 0),
                                                     DepartureTime = new DateTime(2017, 5, 16, 3, 0, 0), Station = stationTwo };
            routeStationsList.Add(routeStationStart);
            routeStationsList.Add(routeStationEnd);

            route.Stations = routeStationsList;
            route.IsDeleted = false;
            routesList.Add(route);


            context.Set<Place>().AddRange(places);
            context.Set<Carriage>().AddRange(carriages);
            context.Set<Train>().AddRange(trainsList);
            context.Set<Route>().AddRange(routesList);
            context.Set<RouteStation>().AddRange(routeStationsList);
            context.Set<Station>().AddRange(stationsList);

            base.Seed(context);
        }
    }
}