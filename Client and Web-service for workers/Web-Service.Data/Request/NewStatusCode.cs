namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс для установки нового статуса
    /// </summary>
    public class NewStatusCode
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
        /// <summary>
        /// Код нового статуса
        /// </summary>
        public string StatusCode { get; set; }
    }
}
