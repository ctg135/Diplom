using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class ClientMock : IClientModel
    {
        public ClientMock()
        {
            Authorization = new AuthoMock();
        }
        public IAuthorizationModel Authorization { get; set; }
    }
}
