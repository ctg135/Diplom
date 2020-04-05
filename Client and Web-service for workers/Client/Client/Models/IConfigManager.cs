using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public interface IConfigManager
    {
        string GetItem(string Item);
        void SetItem(string Item, string Value);
    }
}
