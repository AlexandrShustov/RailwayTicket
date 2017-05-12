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

            CreateMap<Route, RouteViewModel>()
                .ForMember(dest => dest.ArriveTime, src => src.MapFrom(r => r.Stations.Last().ArriveTime.Value))
                .ForMember(dest => dest.DepartureTime, src => src.MapFrom(r => r.Stations.First().DepartureTime.Value))
                .ForMember(dest => dest.FirstStationName, src => src.MapFrom(r => r.Stations.First().Station.Name))
                .ForMember(dest => dest.LastStationName, src => src.MapFrom(r => r.Stations.Last().Station.Name))
                .ForMember(dest => dest.FreePlacesCount,
                    src => src.MapFrom(r => r.Train.Carriages.Sum(t => t.Places.Count(p => p.IsFree))));

            CreateMap<RouteStationCreateViewModel, RouteStation>()
                .ForMember(dest => dest.Station,
                    src => src.MapFrom(r => r.AllStations.First(rs => rs.Id == r.SelectedStation)))
                .ForMember(dest => dest.ArriveTime,
                    src => src.MapFrom(rs => rs.ArriveDate.Add(rs.ArriveTime.TimeOfDay)))
                .ForMember(dest => dest.DepartureTime,
                    src => src.MapFrom(rs => rs.DepartureDate.Add(rs.DepartureTime.TimeOfDay)));

            CreateMap<StationViewModel, Station>();

            CreateMap<Route, DetailsRouteViewModel>()
                .ForMember(dest => dest.CarriagesCount, src => src.MapFrom(r => r.Train.Carriages.Count))
                .ForMember(dest => dest.CommonCarriagesFreePlaces,
                    src =>
                        src.MapFrom(
                            r =>
                                r.Train.Carriages.Where(c => c.CarriageType == CarriageType.Lux)
                                    .Sum(c => c.Places.Count(p => p.IsFree))))
                .ForMember(dest => dest.CompartmentCarriagesFreePlaces,
                    src =>
                        src.MapFrom(
                            r =>
                                r.Train.Carriages.Where(c => c.CarriageType == CarriageType.Compartments)
                                    .Sum(c => c.Places.Count(p => p.IsFree))))
                .ForMember(dest => dest.ReservedSeatCarriagesFreePlaces,
                    src =>
                        src.MapFrom(
                            r =>
                                r.Train.Carriages.Where(c => c.CarriageType == CarriageType.ReservedSeat)
                                    .Sum(c => c.Places.Count(p => p.IsFree))))
                .ForMember(dest => dest.Name,
                    src => src.MapFrom(r => r.Stations.First().Station.Name + "-" + r.Stations.Last().Station.Name))
                .ForMember(dest => dest.TrainNumber, src => src.MapFrom(r => r.Train.Number))
                .ForMember(dest => dest.RouteStations, src => src.MapFrom(r => r.Stations));

            CreateMap<TicketViewModel, Ticket>();
        }

        private List<Place> GetCarriagePlaces(CarriageViewModel carriageVm)
        {
            var list = new List<Place>();

            if (GetCarriageTypeByString(carriageVm.SelectedType) == CarriageType.Compartments)
            {
                for (int i = 0; i < 40; i++)
                {
                    list.Add(new Place { IsFree = true, Number = i + 1 });
                }
            }

            if (GetCarriageTypeByString(carriageVm.SelectedType) == CarriageType.ReservedSeat)
            {
                for (int i = 0; i < 54; i++)
                {
                    list.Add(new Place { IsFree = true, Number = i + 1 });
                }
            }

            if (GetCarriageTypeByString(carriageVm.SelectedType) == CarriageType.Lux)
            {
                for (int i = 0; i < 20; i++)
                {
                    list.Add(new Place { IsFree = true, Number = i + 1 });
                }
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