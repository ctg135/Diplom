namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс для запроса статуса пользователя
    /// </summary>
    public class StatusUserRequest
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
    }
}
