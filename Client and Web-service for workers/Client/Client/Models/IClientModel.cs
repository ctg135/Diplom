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
        /// Производит выборку данных для планов
        /// </summary>
        IPlansModel Plans { get; }
        /// <summary>
        /// Получение коллекции всех статусов
        /// </summary>
        /// <returns>Колеекция статусов</returns>
        Task<List<Status>>  GetStatuses();
        /// <summary>
        /// Получает информацию о работнике
        /// </summary>
        /// <returns></returns>
        Task<Worker> GetWorkerInfo();
        /// <summary>
        /// Получение информации о последнем установленном статусе
        /// </summary>
        /// <returns></returns>
        Task<StatusCode> GetLastStatusCode();
        /// <summary>
        /// Установка статуса работника
        /// </summary>
        /// <param name="Code">Код статуса</param>
        /// <returns></returns>
        Task SetStatus(string Code);
    }
}
