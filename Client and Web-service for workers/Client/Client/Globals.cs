using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;

namespace Client
{
    public static class Globals
    {
        /// <summary>
        /// Экхепляр для работы с конфигурацией приложения
        /// </summary>
        public static IConfigManager Config { get; set; } = new ConfigMock();
    }
}
