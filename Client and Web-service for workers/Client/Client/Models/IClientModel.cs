using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.DataModels;

namespace Client.Models
{
    /// <summary>
    /// Интерфейс для получения данных от сервера
    /// </summary>
    interface IClientModel
    {
        /// <summary>
        /// Производит авторизацию
        /// </summary>
        IAuthorizationModel Authorization { get; }
        /// <summary>
        /// Получение коллекции всех статусов
        /// </summary>
        /// <returns>Колеекция статусов</returns>
        Task<IEnumerable<Status>>  GetStatuses();
        /// <summary>
        /// Получает информацию о работнике
        /// </summary>
        /// <returns></returns>
        Task<Worker> GetWorkerInfo();
    }
}
