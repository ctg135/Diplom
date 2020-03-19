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
        
    }
}