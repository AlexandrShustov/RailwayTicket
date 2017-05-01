using Domain.Entities;
using WebUI.Identity;

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
        } 
    }
}