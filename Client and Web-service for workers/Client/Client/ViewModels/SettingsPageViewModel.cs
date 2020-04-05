using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Client.Models;
namespace Client.ViewModels
{
    class SettingsPageViewModel : BaseViewModel
    {
        public string Server
        {
            get
            {
                return Globals.Config.GetItem("Server");
            }
            set
            {
                Globals.Config.SetItem("Server", value);
            }
        }
        public SettingsPageViewModel()
        {

        }
    }
}
