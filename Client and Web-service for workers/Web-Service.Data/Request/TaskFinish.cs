namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс с данными для заверешения задачи
    /// </summary>
    public class TaskFinish
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
        /// <summary>
        /// Номер выполняемой задачи
        /// </summary>
        public string TaskId { get; set; }
    }
}
