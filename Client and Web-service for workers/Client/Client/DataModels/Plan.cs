using System;
using System.Collections.Generic;
using System.Text;

namespace Client.DataModels
{
    public class Plan
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string StartOfDay { get; set; }
        public string EndOfDay { get; set; }
        public string Total { get; set; }
    }
}
