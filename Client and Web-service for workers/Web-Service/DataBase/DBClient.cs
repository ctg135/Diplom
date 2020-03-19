using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Service.DataBase
{
    /// <summary>
    /// Предоставляет клиент для работы с базой данных
    /// </summary>
    public static class DBClient
    {
        /// <summary>
        /// Экземпляр для работы с базой данных
        /// </summary>
        public static IDBWorker DB { get; set; }
    }
}