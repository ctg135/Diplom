using System.Collections.Generic;
using Newtonsoft.Json;
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
        /// 
        /// </summary>
        public static int PeriodCheckConnection
        {
            get
            {
                return Properties.Settings.Default.PeriodCheckConnection;
            }
        }
        public static string Status_NotStated
        {
            get
            {
                return Properties.Settings.Default.Status_NotStated;
            }
        }
        public static string DisconnectTime
        {
            get
            {
                return Properties.Settings.Default.DisconnectTime;
            }
        }
        public static string Status_Finished
        {
            get
            {
                return Properties.Settings.Default.Status_Finished;
            }
        }
    }
}