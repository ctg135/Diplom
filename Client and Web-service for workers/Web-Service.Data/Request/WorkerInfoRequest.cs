namespace Web_Service.Data.Request
{
    /// <summary>
    /// Клас для получения инофрмации о работнике
    /// </summary>
    public class WorkerInfoRequest
    {
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public string Session { get; set; }
    }
}
