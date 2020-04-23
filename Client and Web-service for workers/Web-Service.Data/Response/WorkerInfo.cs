namespace Web_Service.Data.Response
{
    /// <summary>
    /// Класс для получения информации о работнике (пользователе)
    /// </summary>
    public class WorkerInfo
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Отчество полдьзователя
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// Отдел пользователя
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Должность пользователя
        /// </summary>
        public string Position { get; set; }
    }
}
