using Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.LoginService
{
    public class LoginToken<T> : ILoginToken where T : IUser
    {
        //public long Id { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public UserRoles UserRole { get; set; }

        //public LoginToken(T user)
        //{
        //    Type type = user.GetType();
        //    Id = (long)type.GetProperty("Id").GetValue(user);
        //    UserName = (string)type.GetProperty("UserName").GetValue(user);
        //    Email = (string)type.GetProperty("Email").GetValue(user);
        //    UserRole = (UserRoles)type.GetProperty("UserRole").GetValue(user);
        //}
        public T User { get; set; }

        public LoginToken(T user)
        {
            User = user;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
