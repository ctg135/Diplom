using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Service.Controllers
{
    public static class Authentication
    {
        public static AuthenticationResult Authenticate(string Session, string ClientInfo)
        {


            return AuthenticationResult.Ok;
        }
    }
}