using BL.LoginService;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    public abstract class LoggedInControllerBase<T>:ControllerBase where T:IUser
    {
        internal LoginToken<T> DesirializeToken()
        {
            var string_token = User.Claims.Where(c => c.Type == "LoginToken").FirstOrDefault().Value;
            return JsonConvert.DeserializeObject<LoginToken<T>>(string_token);
        }
    }
}
