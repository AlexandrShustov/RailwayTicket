using Domain.Entities;
using WebUI.Identity;

namespace WebUI.Infrastructure
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<IdentityUser, User>();
            CreateMap<User, IdentityUser>();

            CreateMap<Role, IdentityRole>();
            CreateMap<IdentityRole, Role>();
        } 
    }
}