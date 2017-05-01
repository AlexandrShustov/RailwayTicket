using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        private ICollection<Role> _roles;

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            set { _roles = value; }
        }
    }
}
