using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;

namespace Client
{
    public static class Globals
    {
        public static IConfigManager Config { get; set; } = new ConfigMock();
    }
}
