namespace Web_Service.Data.Response
{
    /// <summary>
    /// Класс представления одного дня графика
    /// </summary>
    public class Plan
    {
        /// <summary>
        /// Дата графика
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Начало дня
        /// </summary>
        public string DayStart { get; set; }
        /// <summary>
        /// Конец дня
        /// </summary>
        public string DayEnd { get; set; }
        /// <summary>
        /// Тип графика
        /// </summary>
        public string PlanCode { get; set; }
    }
}
