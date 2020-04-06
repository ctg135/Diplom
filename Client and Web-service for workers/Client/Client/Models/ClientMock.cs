using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.DataModels;

namespace Client.Models
{
    class ClientMock : IClientModel
    {
        public ClientMock()
        {
            Authorization = new AuthoMock();
        }
        public IAuthorizationModel Authorization { get; set; }

        public Task<IEnumerable<Status>> GetStatuses()
        {
            throw new NotImplementedException();
        }

        public Task<Worker> GetWorkerInfo()
        {
            throw new NotImplementedException();
        }
    }
}
