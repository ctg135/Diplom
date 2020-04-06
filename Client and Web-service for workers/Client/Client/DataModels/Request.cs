using System;
using System.Collections.Generic;
using System.Text;

namespace Client.DataModels
{
    public class Request
    {
        public string Session { get; set; }
        public object Query { get; set; }
    }
}
