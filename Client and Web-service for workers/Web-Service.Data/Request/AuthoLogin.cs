namespace Web_Service.Data.Request
{
    /// <summary>
    /// Класс для авторизации пользователя
    /// </summary>
    public class AuthoLogin
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
