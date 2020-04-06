using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    /// <summary>
    /// Результаты авторизации
    /// </summary>
    enum AuthorizationResult
    {
        Ok,
        Error
    }
    interface IAuthorizationModel
    {
        /// <summary>
        /// Установка сессии по паролю и логину
        /// </summary>
        /// <param name="Login">Логин клиента</param>
        /// <param name="Password">Пароль клиента</param>
        /// <returns>Резуьтат аутентфикации</returns>
        Task<AuthorizationResult> Authorization(string Login, string Password);
        /// <summary>
        /// Авторизация по сессии
        /// </summary>
        /// <returns>Резуьтат аутентфикации</returns>
        Task<AuthorizationResult> Authorization();
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        string Session { get; }
    }
}
