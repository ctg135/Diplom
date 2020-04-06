using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using Client.DataModels;

namespace Client
{
    public static class Globals
    {
        /// <summary>
        /// Экхепляр для работы с конфигурацией приложения
        /// </summary>
        public static IConfigManager Config { get; set; } = new ConfigMock();
        public static Worker WorkerInfo { get; set; }
        public static StatusCode WorkerStatus { get; set; }
        public static List<Status> Statuses { get; set; }
    }
}
