using System.Collections.Generic;
using System.Data;

namespace Web_Service.DataBase
{
    /// <summary>
    /// Предоставляет интерфейс работы с базой данных
    /// </summary>
    public interface IDBWorker
    {
        /// <summary>
        /// Функция выполения запроса к базе данных
        /// </summary>
        /// <param name="query">Строка запроса к базе данных</param>
        /// <returns>Возвращает таблицу с результатом запроса</returns>
        /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
        DataTable MakeQuery(string query);
        /// <summary>
        /// Функция выполения команды к базе данных
        /// </summary>
        /// <param name="query">Строка клманды к базе данных</param>
        /// <exception cref="System.Exception">Ошибка команды, при её наличии</exception>
        void ExecuteQuery(string query);
        /// <summary>
        /// Функция выполения проверки подключения к базе данных
        /// </summary>
        /// <exception cref="System.Exception">Ошибка команды</exception>
        bool CheckConnection();
        /// <summary>
        /// Возвращает таблицу из базы данных в виде DataTable с содержимым таблицы
        /// </summary>
        /// <param name="TableName">Имя таблицы для выборки</param>
        /// <returns>Экземпяр DataTable с содержимым таблицы</returns>
        /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
        DataTable SelectTable(string TableName);
        /// <summary>
        /// Функция для изменения записи в базе данных
        /// </summary>
        /// <param name="TableName">Имя таблицы</param>
        /// <param name="Id">Идентификатор записи</param>
        /// <param name="newValues">Словарь с новыми значениями типа { Поле - Новое значение }</param>
        void UpdateTable(string TableName, string Id, Dictionary<string, string> newValues);
        /// <summary>
        /// Удаляет записи из базы данных
        /// </summary>
        /// <param name="TableName">Название таблицы</param>
        /// <param name="Ids">Список идентификаторов</param>
        void Delete(string TableName, IEnumerable<string> Ids);
        /// <summary>
        /// Вставляет записи в таблицу базы данных
        /// </summary>
        /// <param name="TableName">Название таблицы</param>
        /// <param name="newValues">Словарь полей типа "Поле" - "Значение"</param>
        void Insert(string TableName, Dictionary<string, string> newValues);
    }
}
