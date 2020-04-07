using System;
using System.Collections.Generic;
using System.Text;

namespace Client.DataModels
{
    public class StatusCode
    {
        public string Code { get; set; }
        public string LastUpdate { get; set; }
        public static StatusCode Empty()
        {
            return new StatusCode() { Code = "0", LastUpdate = "-" };
        }
    }
}
