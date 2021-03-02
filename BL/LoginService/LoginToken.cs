using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.LoginService
{
    public class LoginToken<T> : ILoginToken where T : IUser
    {
        public T User { get; set; }

        public LoginToken(T user)
        {
            User = user;
        }
    }
}
