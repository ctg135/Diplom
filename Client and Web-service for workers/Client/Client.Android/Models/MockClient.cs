using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Client.Models;
using Client.DataModels;
using System.Threading.Tasks;

namespace Client.Droid.Models
{
    public class MockClient : IClientModel
    {
        public string Session { get; set; }
        public string Server { get; set; }

        public Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            if (Login == Password) return Task.FromResult(AuthorizationResult.Ok);
            else return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> Authorization()
        {
            return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> CheckConnect()
        {
            return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<StatusCode> GetLastStatusCode()
        {
            return Task.FromResult(new StatusCode() { Code = "2", LastUpdate = DateTime.Now.ToString() });
        }

        public Task<List<Plan>> GetPlans(DateTime Start, DateTime End)
        {
            throw new NotImplementedException();

            List<Plan> plans = new List<Plan>();

            for (int i = 0; Start < End; i++, Start = Start.AddDays(1))
            {
                plans.Add(new Plan() { Date = Start.ToString(), Id = i.ToString(), StartOfDay = "8:30", EndOfDay = "9:30", Total = "1" });
            }

            return Task.FromResult(plans);
        }

        public Task<List<Status>> GetStatuses()
        {
            throw new NotImplementedException();
        }

        public Task<Plan> GetTodayPlan()
        {
            throw new NotImplementedException();
        }

        public Task<Worker> GetWorkerInfo()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSetStatusClientError(string ErrorMessage)
        {
            throw new NotImplementedException();
        }

        public Task SetStatus(string Code)
        {
            throw new NotImplementedException();
        }
    }
}