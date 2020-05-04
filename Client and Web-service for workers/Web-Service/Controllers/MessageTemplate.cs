using System.Net;
using System.Net.Http;

namespace Web_Service.Controllers
{
    /// <summary>
    /// Фабрика для создания шаблонных сообщений
    /// </summary>
    public class MessageTemplate
    {
        /// <summary>
        /// Сообщение о плохой ошибке
        /// </summary>
        public static HttpResponseMessage BadMessage
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Некорректное содержимое сообщения\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        /// <summary>
        /// Сообщение об ошибке сериализации
        /// </summary>
        public static HttpResponseMessage SerializationError
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Ошибка сериализации запроса\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        /// <summary>
        /// Сообщение о ошибке обработки сообщения
        /// </summary>
        public static HttpResponseMessage InternalError
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Ошибка обработки сообщения\"}"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        /// <summary>
        /// Сообщение об ошибке обновления статуса
        /// </summary>
        public static HttpResponseMessage StatusSet_StatusNotUpdateble
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Статус не может быть обновлён\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        /// <summary>
        /// Сообщение об ошибке установки статуса
        /// </summary>
        public static HttpResponseMessage StatusSet_BadStatusCodeGiven
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Статус не может быть установлен\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        /// <summary>
        /// Сообщение об ошибке во время создания сессии
        /// </summary>
        public static HttpResponseMessage SessionNotCreated
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Сессия не была создана\"}"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        /// <summary>
        /// Сообщение о ненайденной сессии при авторизации
        /// </summary>
        public static HttpResponseMessage SessionNotFound
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Сессия не была найдена\"}"),
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }
        }
        /// <summary>
        /// Сообщение о ненайденном клиенте при авторизации
        /// </summary>   
        public static HttpResponseMessage ClientNotFound
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Клиент не был найден\"}"),
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }
        }
        /// <summary>
        /// Сообщение о ненайденном сотруднике при поиске
        /// </summary>
        public static HttpResponseMessage WorkerNotFound
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Работник с таким номером не был найден\"}"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        /// <summary>
        /// Сообщение о полученных неправильных датах
        /// </summary>
        public static HttpResponseMessage BadDatesGived
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Неправильные значения даты\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        /// <summary>
        /// Сообщение о некорректной стадии задачи
        /// </summary>
        public static HttpResponseMessage BadTaskStage
        {
            get
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Такая стадия не может быть установлена\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}