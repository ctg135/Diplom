using System;
using System.Collections.Generic;
using System.Text;

namespace Client.DataModels
{
    public class Plan1
    {
        public string DateSet { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string TypePlan { get; set; }
        public static Plan1 Empty()
        {
            return new Plan1() { DateSet = "-", EndDay = "-", StartDay = "-", TypePlan = "0" };
        }
    }
}
