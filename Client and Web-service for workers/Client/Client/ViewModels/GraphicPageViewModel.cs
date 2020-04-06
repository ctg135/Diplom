using System;
using System.Collections.Generic;
using System.Text;
using Client.DataModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Globalization;

namespace Client.ViewModels
{
    class GraphicPageViewModel : BaseViewModel
    {
        public List<Plan> Plans { get; set; }

        public GraphicPageViewModel()
        {
            Plans = new List<Plan>() 
            { 
                new Plan() { Date = "12.12.2012", StartOfDay = "8:30", EndOfDay = "17:00", Total = "8.5" },
                new Plan() { Date = "13.12.2012", StartOfDay = "8:30", EndOfDay = "17:00", Total = "8.5" }
            };
        }
    }
}
