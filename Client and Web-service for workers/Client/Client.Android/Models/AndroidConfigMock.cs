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

namespace Client.Droid.Models
{
    class AndroidConfigMock : IConfigManager
    {
        private Dictionary<string, string> values { get; set; } = new Dictionary<string, string>();
        public string GetItem(string Item)
        {
            if (values.ContainsKey(Item))
            {
                return values[Item];
            }
            else
            {
                return "";
            }
        }
        public void SetItem(string Item, string Value)
        {
            if (values.ContainsKey(Item))
            {
                values[Item] = Value;
            }
            else
            {
                values.Add(Item, Value);
            }
        }
    }
}