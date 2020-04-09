using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public interface IConfigManager
    {
        /// <summary>
        /// Выбор значения из конфигурации
        /// </summary>
        /// <param name="Item">Элемнт для выбора значения - <c>Key</c></param>
        /// <returns>Значение</returns>
        Task<string> GetItem(string Item);
        /// <summary>
        /// Установка значению элемента в конфигурации
        /// </summary>
        /// <param name="Item">Элемент для установки - <c>Key</c></param>
        /// <param name="Value">Значение - <c>Value</c></param>
        /// <returns>Task</returns>
        Task SetItem(string Item, string Value);
    }
}
