using Microsoft.AspNet.Identity;

namespace DAL
{
    public class user : IUser
    {
        public string Id { get; }
        public string UserName { get; set; }
    }
}