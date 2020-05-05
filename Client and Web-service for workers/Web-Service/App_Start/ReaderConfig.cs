using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Web_Service
{
    /// <summary>
    /// Считывает конфигурационный файл
    /// </summary>
    public class ReaderConfig
    {
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public static string ConnectionStringDB
        {
            get
            {
                var settings = Properties.Settings.Default;
                string Res = $"Server={settings.DBServer};";
                Res += $"Port={settings.DBPort};";
                Res += $"Database={settings.DBDatabase};";
                Res += $"Uid={settings.DBUid};";
                Res += $"Pwd={settings.DBPwd};";
                return Res;
            }
        }
        /// <summary>
        /// Получает список "длительных статусов"
        /// </summary>
        public static IEnumerable<string> LongStatuses
        {
            get
            {
                List<string> statuses = JsonConvert.DeserializeObject<List<string>>(Properties.Settings.Default.LongStatuses);
                return statuses;
            }
        }
        /// <summary>
        /// Период проверки обновления сессий на активные подключения
        /// </summary>
        public static int PeriodCheckConnection
        {
            get
            {
                int period = 0;
                DateTime Period = DateTime.Parse(Properties.Settings.Default.PeriodCheckConnection);

                period += Period.Millisecond;
                period += Period.Second * 1000;
                period += Period.Minute * 60 * 1000;
                period += Period.Hour * 60 * 60 * 1000;

                return period;
            }
        }
        /// <summary>
        /// Код неустановленного статуса
        /// </summary>
        public static string Status_NotStated
        {
            get
            {
                return Properties.Settings.Default.Status_NotStated;
            }
        }
        /// <summary>
        /// Время жизни сессии
        /// </summary>
        public static string DisconnectTime
        {
            get
            {
                return Properties.Settings.Default.DisconnectTime;
            }
        }
        /// <summary>
        /// Код статуса завершенного рабочего дня
        /// </summary>
        public static string Status_Finished
        {
            get
            {
                return Properties.Settings.Default.Status_Finished;
            }
        }
        /// <summary>
        /// Словарь для пар { Код графика - код статуса этого графика }
        /// </summary>
        public static Dictionary<string, string> GraphicCode_StatusCode
        {
            get
            { 
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.GraphicCode_StatusCode);
            }
        }
        /// <summary>
        /// Возвращает код рабочего дня в графиках
        /// </summary>
        public static string Day_Working
        {
            get
            {
                return Properties.Settings.Default.Day_Working;
            }
        }
        /// <summary>
        /// Возвращает код выходного дня в графиках
        /// </summary>
        public static string Day_DayOff
        {
            get
            {
                return Properties.Settings.Default.Day_DayOff;
            }
        }
        /// <summary>
        /// Значение статуса выходного
        /// </summary>
        public static string State_DayOff
        {
            get
            {
                return Properties.Settings.Default.State_DayOff;
            }
        }
        /// <summary>
        /// Значение стадии принятой к исполнению задачи
        /// </summary>
        public static string Task_Accepted
        {
            get
            {
                return Properties.Settings.Default.Task_Accepted;
            }
        }
        /// <summary>
        /// Значение стадии не принятой к исполнению задачи
        /// </summary>
        public static string Task_NotAccepted
        {
            get
            {
                return Properties.Settings.Default.Task_NotAccepted;
            }
        }
        /// <summary>
        /// Значение стадии завершенной задачи
        /// </summary>
        public static string Task_Completed
        {
            get
            {
                return Properties.Settings.Default.Task_Completed;
            }
        }
        /// <summary>
        /// Возвращает рабочее состояние работника
        /// </summary>
        public static string State_Working
        {
            get
            {
                return Properties.Settings.Default.State_Working;
            }
        }
        /// <summary>
        /// Состояние "На перерыве"
        /// </summary>
        public static string State_Paused
        {
            get
            {
                return Properties.Settings.Default.State_Paused;
            }
        }
    }
}