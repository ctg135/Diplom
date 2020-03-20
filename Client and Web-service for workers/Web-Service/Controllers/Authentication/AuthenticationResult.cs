using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Service.Controllers
{
    public enum AuthenticationResult
    {
        Ok,
        SessionNotFound,
        ClientNotCorrect
    }
}