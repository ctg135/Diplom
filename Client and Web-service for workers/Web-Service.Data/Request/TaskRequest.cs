using System.Collections.Generic;

namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс запроса заданий
    /// </summary>
    public class TaskRequest
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
        /// <summary>
        /// Стадии задач для отбора
        /// </summary>
        public IEnumerable<string> TaskStages { get; set; }
        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public string DateCreation { get; set; }
    }
}
