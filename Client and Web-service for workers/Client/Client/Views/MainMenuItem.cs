using System;
using Xamarin.Forms;

namespace Client.Views
{

    public class MainMenuItem
    {
        public MainMenuItem()
        {
            TargetType = typeof(MainMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
        public ImageSource IconSource { get; set; }
    }
}