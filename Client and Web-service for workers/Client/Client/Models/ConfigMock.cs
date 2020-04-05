using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class ConfigMock : IConfigManager
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
