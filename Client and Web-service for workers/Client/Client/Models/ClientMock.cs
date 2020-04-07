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
            // ТЯВА
            Authorization = new AuthoMock();
            Plans = new PlansMock();
        }
        public IAuthorizationModel Authorization { get; set; }
        public IPlansModel Plans { get; set; }
        public async Task<List<Status>> GetStatuses()
        {
            List<Status> statuses = new List<Status>();
            statuses.Add(new Status() { Title = "Не установлен", Code = "1", Description = "...." });
            statuses.Add(new Status() { Title = "На работе", Code = "2", Description = "...." } );
            statuses.Add(new Status() { Title = "Перерыв", Code = "3", Description = "...." } );
            statuses.Add(new Status() { Title = "Рабочий день закончен", Code = "5", Description = "...." } );
            return await Task.FromResult(statuses);
        }

        public async Task<Worker> GetWorkerInfo()
        {
            return await Task.FromResult(new Worker() { Name = "Тостер",  Surname = "Тестер" });
        }

        private StatusCode LastStatus { get; set; } = new StatusCode() { Code = "1", LastUpdate = DateTime.Now.ToString("g") };

        

        public async Task SetStatus(string Code)
        {
            LastStatus = new StatusCode() { Code = Code, LastUpdate = DateTime.Now.ToString("g") };
        }
        public async Task<StatusCode> GetLastStatusCode()
        {
            return await Task.FromResult(LastStatus);
        }
    }
}
