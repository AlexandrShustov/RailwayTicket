using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            var roles = new List<string> {"moder", "user"};

            Roles = new SelectList(roles);
        } 

        public SelectList Roles { get; set; }

        public string SelectedRole { get; set; }

        public User User { get; set; }
    }
}