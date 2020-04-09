using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<string> GetItem(string Item)
        {
            if (values.ContainsKey(Item))
            {
                return await Task.FromResult(values[Item]);
            }
            else
            {
                return await Task.FromResult("");
            }
        }
        public async Task SetItem(string Item, string Value)
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