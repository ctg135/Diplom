using Client.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public interface IPlanLoader
    {
        /// <summary>
        /// Устанавливает список планов в память
        /// </summary>
        /// <param name="Plans">Список планов</param>
        /// <returns></returns>
        Task SetPlans(List<Plan> Plans);
        /// <summary>
        /// Считывает все планы из памяти
        /// </summary>
        /// <returns></returns>
        Task<List<Plan>> GetPlans();
        /// <summary>
        /// Очищает все планы в памяти
        /// </summary>
        /// <returns></returns>
        Task ClearPlans();
    }
}
