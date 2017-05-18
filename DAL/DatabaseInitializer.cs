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

            var carriage = new Carriage{ Number = 1, Places = places };
            carriage.CarriageType = CarriageType.Compartments;
            carriage.IsDeleted = false;
            carriages.Add(carriage);
            
            var train = new Train{ Carriages = carriages, Number = 444 };
            trainsList.Add(train);
            
            var route = new Route();
            route.Train = train;
     
            var station1 = new Station {Name = "Kharkov"};
            var station2 = new Station { Name = "Kiev" };
            var station3 = new Station { Name = "Lviv" };
            var station4 = new Station {Name = "Ivano-Frankivsk"};
            var station5 = new Station {Name = "Odessa"};
            var station6 = new Station { Name = "Kherson" };
            var station7 = new Station { Name = "Dnipro" };

            var station8 = new Station {Name = "Uzhgorod"};
            var station9 = new Station {Name = "Berlin"};
            var station10 = new Station { Name = "Dortmund" };
            var station11 = new Station { Name = "Warshava" };
            var station12 = new Station { Name = "Strasburg" };
            var station13 = new Station { Name = "Amsterdam" };

            stationsList.Add(station1);
            stationsList.Add(station2);
            stationsList.Add(station3);
            stationsList.Add(station4);
            stationsList.Add(station5);
            stationsList.Add(station6);
            stationsList.Add(station7);
            stationsList.Add(station8);
            stationsList.Add(station9);
            stationsList.Add(station10);
            stationsList.Add(station11);
            stationsList.Add(station12);
            stationsList.Add(station13);

            var routeStation1 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 15, 23, 23, 0),
                DepartureTime = new DateTime(2017, 5, 17, 1, 15, 0),
                Station = station1
            };
            var routeStation2 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 17, 2, 30, 0),
                DepartureTime = new DateTime(2017, 5, 17, 3, 0, 0),
                Station = station2
            };
            var routeStation3 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 17, 4, 50, 0),
                DepartureTime = new DateTime(2017, 5, 17, 5, 0, 0),
                Station = station3
            };
            var routeStation4 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 17, 8, 50, 0),
                DepartureTime = new DateTime(2017, 5, 17, 8, 55, 0),
                Station = station4
            };
            var routeStation5 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 17, 10, 13, 0),
                DepartureTime = new DateTime(2017, 5, 17, 10, 15, 0),
                Station = station5
            };
            var routeStation6 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 17, 12, 6, 0),
                DepartureTime = new DateTime(2017, 5, 17, 12, 30, 0),
                Station = station6
            };
            var routeStation7 = new RouteStation
            {
                ArriveTime = new DateTime(2017, 5, 17, 16, 22, 0),
                DepartureTime = new DateTime(2017, 5, 17, 17, 30, 0),
                Station = station7
            };

            routeStationsList.Add(routeStation1);
            routeStationsList.Add(routeStation2);
            routeStationsList.Add(routeStation3);
            routeStationsList.Add(routeStation4);
            routeStationsList.Add(routeStation5);
            routeStationsList.Add(routeStation6);
            routeStationsList.Add(routeStation7);

            route.Stations = routeStationsList;
            route.IsDeleted = false;
            routesList.Add(route);

            var feedbackList = new List<Feedback>();
            var feedback1 = new Feedback { FeedbackText = "Not bad :)", PostingDate = DateTime.UtcNow, RelatedUserId = commonUser.UserId };
            var feedback2 = new Feedback { FeedbackText = "plz add a some cool features", PostingDate = DateTime.UtcNow, RelatedUserId = moderUser.UserId };
            feedbackList.Add(feedback1);
            feedbackList.Add(feedback2);

            context.Set<Place>().AddRange(places);
            context.Set<Carriage>().AddRange(carriages);
            context.Set<Train>().AddRange(trainsList);
            context.Set<Route>().AddRange(routesList);
            context.Set<RouteStation>().AddRange(routeStationsList);
            context.Set<Station>().AddRange(stationsList);
            context.Set<Feedback>().AddRange(feedbackList);

            base.Seed(context);
        }
    }
}