using System.Collections.Generic;

namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс запроса списка планов
    /// </summary>
    public class PlansRequest
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
        /// <summary>
        /// Начальный день запроса
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Конечный день запроса
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Список типов графиков для запроса
        /// </summary>
        public IEnumerable<string> PlanCodes { get; set; }
    }
}
