namespace Web_Service.Data.Response
{
    /// <summary>
    /// Класс с информацией о статусе
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Код статуса
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// Время последнего обновления
        /// </summary>
        public string Updated { get; set; }
    }
}
