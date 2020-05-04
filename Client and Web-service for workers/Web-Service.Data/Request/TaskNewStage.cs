namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс с данными для заверешения задачи
    /// </summary>
    public class TaskNewStage
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
        /// <summary>
        /// Номер выполняемой задачи
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// Новая стадия задачи
        /// </summary>
        public string Stage { get; set; }
    }
}
