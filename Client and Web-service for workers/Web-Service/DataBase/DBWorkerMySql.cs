using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Web_Service.DataBase
{
    /// <summary>
    /// Представляет работу с базой данных MySql
    /// </summary>
    public class DBWorkerMySql : IDBWorker
    {
        /// <summary>
        /// Экземпляр подключения базы данных
        /// </summary>
        private MySqlConnection connection;
        /// <summary>
        /// Экземпляр подключения базы данных
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connection.ConnectionString;
            }
            set
            {
                connection.ConnectionString = value;
            }
        }
        /// <summary>
        /// Конструктор подключения к базе данных
        /// </summary>
        /// <param name="ConnectionString">Строка подключения к базе данных</param>
        public DBWorkerMySql(string ConnectionString)
        {
            connection = new MySqlConnection(ConnectionString);
        }
        /// <summary>
        /// Функция выполения запроса к базе данных
        /// </summary>
        /// <param name="query">Строка запроса к базе данных</param>
        /// <returns>Возвращает таблицу с результатом запроса</returns>
        /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
        public DataTable MakeQuery(string query)
        {
            DataTable table = new DataTable();
            using (MySqlDataAdapter sql = new MySqlDataAdapter(query, connection))
            {
                try
                {
                    connection.Open();
                    sql.Fill(table);
                }
                catch (MySqlException MySqlException)
                {
                    throw new Exception(MySqlException.Message + "\nЗапрос:\n" + query);
                }
                finally
                {
                    connection.Close();
                }
            }
            return table;
        }
        /// <summary>
        /// Функция выполения команды к базе данных
        /// </summary>
        /// <param name="query">Строка клманды к базе данных</param>
        /// <exception cref="System.Exception">Ошибка команды, при её наличии</exception>
        public void ExecuteQuery(string query)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException MySqlException)
                {
                    throw new Exception(MySqlException.Message + "\nЗапрос:\n" + query);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// Функция выполения команды к базе данных
        /// </summary>
        /// <exception cref="System.Exception">Ошибка команды</exception>
        public bool CheckConnection()
        {
            bool isWorking = true;
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                isWorking = false;
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return isWorking;
        }
        /// <summary>
        /// Возвращает таблицу из базы данных в виде DataTable с содержимым таблицы
        /// </summary>
        /// <param name="TableName">Имя таблицы для выборки</param>
        /// <returns>Экземпяр DataTable с содержимым таблицы</returns>
        /// <exception cref="System.Exception">Ошибка запроса, при её наличии</exception>
        public DataTable SelectTable(string TableName)
        {
            return MakeQuery($"SELECT * FROM `{TableName}`");
        }
        /// <summary>
        /// Функция для изменения записи в базе данных
        /// </summary>
        /// <param name="TableName">Имя таблицы</param>
        /// <param name="Id">Идентификатор записи</param>
        /// <param name="newValues">Словарь с новыми значениями типа { Поле - Новое значение }</param>
        public void UpdateTable(string TableName, string Id, Dictionary<string, string> newValues)
        {
            string query = $"UPDATE `{TableName}` SET";
            foreach (var pair in newValues)
            {
                query += $"\n`{pair.Key}` = '{pair.Value}',";
            }
            query = query.Remove(query.Length - 1, 1);
            query += $"\nWHERE `Id` = {Id}";
            ExecuteQuery(query);
        }
        /// <summary>
        /// Удаляет записи из базы данных
        /// </summary>
        /// <param name="TableName">Название таблицы</param>
        /// <param name="Ids">Список идентификаторов</param>
        public void Delete(string TableName, IEnumerable<string> Ids)
        {
            string query = $"DELETE FROM `{TableName}` WHERE `Id` IN (";
            foreach (string id in Ids)
            {
                query += $"'{id}',";
            }
            query = query.Remove(query.Length - 1, 1);
            query += ")";
            ExecuteQuery(query);
        }
        /// <summary>
        /// Вставляет записи в таблицу базы данных
        /// </summary>
        /// <param name="TableName">Название таблицы</param>
        /// <param name="newValues">Словарь полей типа "Поле" - "Значение"</param>
        public void Insert(string TableName, Dictionary<string, string> newValues)
        {
            string query = $"INSERT INTO `{TableName}`\n (";
            foreach (string column in newValues.Keys)
            {
                query += $"`{column}`,";
            }
            query = query.Remove(query.Length - 1, 1);
            query += ")\n VALUES (";
            foreach (string value in newValues.Values)
            {
                query += $"'{value}',";
            }
            query = query.Remove(query.Length - 1, 1);
            query += ")";
            ExecuteQuery(query);
        }
    }
}