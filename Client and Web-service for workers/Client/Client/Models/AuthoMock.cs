using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class AuthoMock : IAuthorizationModel
    {
        public Task<string> Session { get; set; }

        public Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            if (Login == Password) return Task.FromResult(AuthorizationResult.Ok);
            else return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> Authorization()
        {
            return Task.FromResult(AuthorizationResult.Ok);
        }
    }
}
