using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service.Controllers
{
    /// <summary>
    /// Исключение во время авторизации
    /// </summary>
    public class AuthenticationExcecption : Exception
    {
        public AuthenticationExcecption()
        {
        }

        public AuthenticationExcecption(string message) : base(message)
        {
        }

        public AuthenticationExcecption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthenticationExcecption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
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
            string Client     = string.Empty;
            string OldSession = string.Empty;

            try
            {
                string WorkerId    = DBClient.GetWorkerId(Session);
                Client             = DBClient.GetClientInfo(Session);
                OldSession         = DBClient.SearchSession(WorkerId, Client);
            }
            catch(Exception exc)
            {
                Logger.AuthoLog.Fatal(exc, "Ошибка поиска сессии");
                return AuthenticationResult.SessionNotFound;
            }
            // Если сессия найдена и совпадают клиенты
            if (!string.IsNullOrEmpty(Session))
            {
                if (ClientInfo == Client)
                {
                    DBClient.UpdateSession(Session, DateTime.Now);
                    return AuthenticationResult.Ok;
                }
                else
                {
                    return AuthenticationResult.ClientNotFound;
                }
            }
            else
            {
                return AuthenticationResult.SessionNotFound;
            }
        }
        /// <summary>
        /// Создание новой сессии
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="Password">Пароль пользователя</param>
        /// <param name="ClientInfo">Информация о клиенте</param>
        /// <returns>Хэш новой созданной сессии</returns>
        /// <exception cref="Exception">Ошибка аутентификации</exception>
        public static string CreateSession(string Login, string Password, string ClientInfo, DateTime Date)
        {
            var WorkerId = DBClient.GetWorkerId(Login, Password);

            if (string.IsNullOrEmpty(WorkerId))
            {
                throw new AuthenticationExcecption("Неверный логин или пароль");
            }

            var PrevSession = DBClient.SearchSession(WorkerId, ClientInfo);

            if(string.IsNullOrEmpty(PrevSession))
            {
                var hash = Encoding.UTF8.GetBytes(Login + Password + ClientInfo + DateTime.Now.ToString());

                var bytes = SHA1.Create().ComputeHash(hash);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                string sessionHash = builder.ToString();

                DBClient.InsertSession(WorkerId, sessionHash, ClientInfo, Date);

                return sessionHash;
            }
            else
            {
                return PrevSession;
            }
        }
    }
}