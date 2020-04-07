using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.DataModels;

namespace Client.Models
{
    /// <summary>
    /// Интерфейс для работы с графиком работника
    /// </summary>
    interface IPlansModel
    {
        /// <summary>
        /// Получение планов сотрудника по датам
        /// </summary>
        /// <param name="Start">Начальная дата</param>
        /// <param name="End">Конечная дата</param>
        /// <returns></returns>
        Task<List<Plan>> GetPlans(DateTime Start, DateTime End);
        /// <summary>
        /// Получение плана на сегодня
        /// </summary>
        /// <returns></returns>
        Task<Plan> GetTodayPlan();
    }
}
