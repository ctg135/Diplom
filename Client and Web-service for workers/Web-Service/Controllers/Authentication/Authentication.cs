using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Service.DataBase;
using System.Security.Cryptography;
using System.Text;
using Web_Service.Loggers;

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
            string WorkerId = string.Empty;
            string Client   = string.Empty;

            try
            {
                WorkerId = DBClient.GetWorkerId(Session);
                Client   = DBClient.GetClientInfo(Session);
            }
            catch(Exception exc)
            {
                Logger.AuthoLog.Error("Ошибка поиска сессии", exc);
                return AuthenticationResult.SessionNotFound;
            }

            if (string.IsNullOrEmpty(WorkerId) || string.IsNullOrEmpty(Client))
                return AuthenticationResult.SessionNotFound;

            if (ClientInfo != Client)
                return AuthenticationResult.ClientNotFound;

            return AuthenticationResult.Ok;
        }
        /// <summary>
        /// Создание новой сессии
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="Password">Пароль пользователя</param>
        /// <param name="ClientInfo">Информация о клиенте</param>
        /// <returns>Хэш новой созданной сессии</returns>
        public static string CreateSessionHash(string Login, string Password, string ClientInfo, DateTime Date)
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