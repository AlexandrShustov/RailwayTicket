using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Abstract;
using Domain.Entities;
using Domain.Enumerations;
using Ninject;
using WebUI.Identity;
using WebUI.Models;

namespace WebUI.Infrastructure
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {

            CreateMap<IdentityUser, User>().ForMember(u => u.UserId, cfg => cfg.MapFrom(iu => iu.Id));
            CreateMap<User, IdentityUser>().ForMember(iu => iu.Id, cfg => cfg.MapFrom(u => u.UserId));

            CreateMap<Role, IdentityRole>();
            CreateMap<IdentityRole, Role>();

            CreateMap<RegisterViewModel, User>();
            CreateMap<Train, TrainViewModel>();
            CreateMap<TrainViewModel, Train>();

            CreateMap<Carriage, CarriageViewModel>()
                .ForMember(dest => dest.Places, src => src.MapFrom(c => c.Places.Count))
                .ForMember(dest => dest.SelectedType, src =>
                {
                    string item = string.Empty;
                });

            CreateMap<CarriageViewModel, Carriage>()
                .ForMember(dest => dest.Places, src => src.MapFrom(c => GetCarriagePlaces(c)))
                .ForMember(dest => dest.CarriageType, src => src.MapFrom(c => GetCarriageTypeByString(c.SelectedType)));

            CreateMap<Route, RouteEditViewModel>();
            CreateMap<RouteEditViewModel, Route>();

            CreateMap<RouteStationCreateViewModel, RouteStation>()
                .ForMember(dest => dest.Station,
                    src => src.MapFrom(r => r.AllStations.First(rs => rs.Id == r.SelectedStation)))
                .ForMember(dest => dest.ArriveTime, 
                    src => src.MapFrom(rs => rs.ArriveDate.Add(rs.ArriveTime.TimeOfDay)))
                .ForMember(dest => dest.DepartureTime,
                    src => src.MapFrom(rs => rs.DepartureDate.Add(rs.DepartureTime.TimeOfDay)));

            CreateMap<StationViewModel, Station>();

        }

        private List<Place> GetCarriagePlaces(CarriageViewModel carriageVm)
        {
            var list = new List<Place>();

            for (int i = 0; i < carriageVm.Places; i++)
            {
                list.Add(new Place {IsFree = true, Number = i + 1});
            }

            return list;
        }

        private CarriageType GetCarriageTypeByString(string type)
        {
            string result = string.Empty;
            Enum.TryParse(type, out CarriageType carriageType);

            return carriageType;
        }
    }
}