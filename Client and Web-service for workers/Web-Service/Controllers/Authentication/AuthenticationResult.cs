namespace Web_Service.Controllers
{
    /// <summary>
    /// Результат аутентификации
    /// </summary>
    public enum AuthenticationResult
    {
        /// <summary>
        /// Аутентификация прошла успешно
        /// </summary>
        Ok,
        /// <summary>
        /// Искомая сессия не была найдена
        /// </summary>
        SessionNotFound,
        /// <summary>
        /// Искомый клиент не был найден
        /// </summary>
        ClientNotFound
    }
}