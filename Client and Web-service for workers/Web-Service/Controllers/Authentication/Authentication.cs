using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Service.DataBase;
using System.Security.Cryptography;
using System.Text;

namespace Web_Service.Controllers
{
    /// <summary>
    /// Класс для проведения авторизации
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// Аутентификация по сессии
        /// </summary>
        /// <param name="Session">Хэш сессии</param>
        /// <param name="ClientInfo">Информация о клиенте</param>
        /// <returns>Результат авторизации</returns>
        public static AuthenticationResult Authenticate(string Session, string ClientInfo)
        {


            return AuthenticationResult.Ok;
        }
        /// <summary>
        /// Создание новой сессии
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="Password">Пароль пользователя</param>
        /// <param name="ClientInfo">Информация о клиенте</param>
        /// <returns>Хэш новой созданной сессии</returns>
        public static string CreateNewSession(string Login, string Password, string ClientInfo, DateTime Date)
        {
            var hash = Encoding.UTF8.GetBytes(Login + Password + ClientInfo + DateTime.Now.ToString());

            var bytes = SHA1.Create().ComputeHash(hash);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }            

            string sessionHash = builder.ToString();

            return sessionHash;
        }
    }
}