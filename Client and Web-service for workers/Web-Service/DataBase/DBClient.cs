using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Service.Models;

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
        /// <summary>
        /// Список длительных статусов
        /// </summary>
        public static List<string> LongStatuses { get; set; }
        /// <summary>
        /// Код неустановленного статуса
        /// </summary>
        public static string State_NotState { get; set; }
        /// <summary>
        /// Код статуса завершённой работы
        /// </summary>
        public static string State_Finished { get; set; }
        /// <summary>
        /// Время отключения неработающей сессии
        /// </summary>
        public static string DisconnectTime { get; set; }
        /// <summary>
        /// Функция записи сессии в базу данных
        /// </summary>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static void CreateSession(string WorkerId, string SessionHash, string ClientInfo, DateTime DateCreation) 
        => DB.Insert("sessions", new Dictionary<string, string>() 
        {
            { "WorkerId",   WorkerId },
            { "Token",      SessionHash },
            { "ClientInfo", ClientInfo },
            { "Created",    DateCreation.ToString("yyyy-MM-dd HH:mm:ss") },
            { "LastUpdate", DateCreation.ToString("yyyy-MM-dd HH:mm:ss") }
        });
        /// <summary>
        /// Возаращает Id работника по данным авторизации
        /// <br><c>string.Empty</c> - в случае ненахождения такого</br>
        /// </summary>
        /// <param name="Login">Логин работника</param>
        /// <param name="Password">Пароль работника</param>
        /// <returns>Id работника</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static string GetWorkerId(string Login, string Password)
        {
            DataTable data = DB.MakeQuery($"SELECT `Id` FROM `workers` WHERE `Login` = '{Login}' AND `Password` = '{Password}';");
            string id = string.Empty;
            if(data.Rows.Count == 1)
            {
                id = data.Rows[0][0].ToString();
            }
            return id;
        }
        /// <summary>
        /// Получение Id работника по сессии
        /// </summary>
        /// <param name="Session">Хэш сессиии</param>
        /// <returns>Id работника</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static string GetWorkerId(string Session)
        {
            string id = string.Empty;

            DataTable data = DB.MakeQuery($"SELECT `WorkerId` FROM `sessions` WHERE `Token` = '{Session}'");
            if (data.Rows.Count == 1)
            {
                id = data.Rows[0][0].ToString();
            }

            return id;
        }
        /// <summary>
        /// Получение клиента работника по сессии
        /// </summary>
        /// <param name="Session">Хэш сессии</param>
        /// <returns>Клиент сотрудника</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static string GetClientInfo(string Session)
        {
            string client = string.Empty;

            DataTable data = DB.MakeQuery($"SELECT `ClientInfo` FROM `sessions` WHERE `Token` = '{Session}'");
            if (data.Rows.Count == 1)
            {
                client = data.Rows[0][0].ToString();
            }

            return client;
        }
        /// <summary>
        /// Функция получения информации о сотруднике
        /// </summary>
        /// <param name="WorkerId">Идентификатор сотрудника</param>
        /// <returns>Информация о сотруднике</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static Worker GetWorker(string WorkerId)
        {
            var data = DB.MakeQuery($"SELECT * FROM `workers` WHERE `Id` = '{WorkerId}'");

            if(data.Rows.Count != 1)
            {
                throw new Exception("Работник не найден");
            }

            Worker worker = new Worker()
            {
                Name        = data.Rows[0]["Name"].ToString(),
                Surname     = data.Rows[0]["Surname"].ToString(),
                Patronymic  = data.Rows[0]["Patronymic"].ToString(),
                BirthDate   = data.Rows[0]["BirthDate"].ToString(),
                Mail        = data.Rows[0]["Mail"].ToString(),
                Position    = data.Rows[0]["Position"].ToString(),
                Rate        = data.Rows[0]["Rate"].ToString(),
                AccessLevel = data.Rows[0]["AccessLevel"].ToString()
            };
            return worker;
        }
        /// <summary>
        /// Функция выбор всех видов статуса работников
        /// </summary>
        /// <returns>Все статусы</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static IEnumerable<Status> GetStatuses()
        {
            List<Status> statuses = new List<Status>();

            DataTable data = DB.SelectTable("statuses");

            foreach(DataRow row in data.Rows)
            {
                statuses.Add(new Status()
                {
                    Code        = row["Code"].ToString(),
                    Title       = row["Title"].ToString(),
                    Description = row["Description"].ToString()
                });
            }

            return statuses;
        }
        /// <summary>
        /// Функция получения статуса работника по id
        /// </summary>
        /// <param name="WorkerId">Идентификатор работника</param>
        /// <returns>Статус работника</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static StatusCode GetStatus(string WorkerId, DateTime CheckDate)
        {
            DataTable data = DB.MakeQuery("SELECT `StatusCode`, `SetDate`, `SetTime` " +
                "FROM `statuslogs` " +
                $"WHERE `WorkerId` = '{WorkerId}' " +
                "ORDER BY `SetDate` DESC, `SetTime` DESC " +
                "LIMIT 1;"
                );

            if(data.Rows.Count == 0)
            {
                return new StatusCode() { Code = State_NotState, LastUpdate = CheckDate.ToString() };
            }
            else if(((DateTime)data.Rows[0]["SetDate"]).ToString("yyyy-MM-dd") != CheckDate.ToString("yyyy-MM-dd"))
            {
                if (!IsLongStatus(data.Rows[0]["StatusCode"].ToString()))
                {
                    return new StatusCode() { Code = State_NotState, LastUpdate = CheckDate.ToString() };
                }
                else
                {
                    return new StatusCode() { Code = data.Rows[0]["StatusCode"].ToString(), LastUpdate = ((DateTime)data.Rows[0]["SetDate"]).ToString("yyyy-MM-dd") + " " + data.Rows[0]["SetTime"].ToString() };
                }
            }
            else
            {
                return new StatusCode() { Code = data.Rows[0]["StatusCode"].ToString(), LastUpdate = ((DateTime)data.Rows[0]["SetDate"]).ToString("yyyy-MM-dd") + " " + data.Rows[0]["SetTime"].ToString() };
            }
        }
        /// <summary>
        /// Указыввает, является ли переданный статус "длительным"
        /// </summary>
        /// <param name="StatusCode">Статус</param>
        /// <returns>Если да, то true</returns>
        private static bool IsLongStatus(string StatusCode)
        {
            bool longstatus = false;
            foreach (string longstate in LongStatuses)
            {
                if (StatusCode == longstate)
                {
                    longstatus = true;
                    break;
                }
            }
            return longstatus;
        }
        /// <summary>
        /// Функция записи статуса в базу данных
        /// </summary>
        /// <param name="WorkerId">Идентификатор работника</param>
        /// <param name="CodeStatus">Код статуса</param>
        /// <param name="Setted">Время установки</param>
        public static void LogStatus(string WorkerId, string CodeStatus, DateTime Setted)
        {
            DB.Insert("statuslogs", new Dictionary<string, string>()
            {
                { "WorkerId", WorkerId },
                { "SetDate", Setted.ToString("yyyy-MM-dd") },
                { "SetTime", Setted.ToString("HH:mm:ss") },
                { "StatusCode", CodeStatus }
            });
        }
        /// <summary>
        /// Функция для получения планов на количсество дней от указанной даты
        /// </summary>
        /// <param name="WorkerId">Идентификатор работника</param>
        /// <param name="StartDay">Начальынй день</param>
        /// <param name="DaysCount">Количество дней</param>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static IEnumerable<Plan> GetPlans(string WorkerId, DateTime StartDay, int DaysCount)
        {
            List<Plan> plans = new List<Plan>();

            DataTable data = DB.MakeQuery($"SELECT * FROM `plans` WHERE `WorkerId` = '{WorkerId}' " +
                $"AND `Date` >= '{DateTime.Now.ToString("yyyy-MM-dd")}' " +
                $"AND `Date` <= '{DateTime.Now.AddDays(DaysCount).ToString("yyyy-MM-dd")}'");

            foreach(DataRow row in data.Rows)
            {
                plans.Add(new Plan() 
                { 
                    Id         = row["Id"].ToString(),
                    WorkerId   = row["WorkerId"].ToString(),
                    Date       = row["Date"].ToString(),
                    StartOfDay = row["StartOfDay"].ToString(),
                    EndOfDay   = row["EndOfDay"].ToString(),
                    Total      = row["Total"].ToString()
                });
            }

            return plans;
        }
        /// <summary>
        /// Функция обновления времени жизни сессии
        /// </summary>
        /// <param name="Session">Хэш сессии</param>
        /// <param name="Time">Время и дата  обновления</param>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static void UpdateSession(string Session, DateTime Time)
        {
            DB.ExecuteQuery($"UPDATE `sessions` SET `LastUpdate` = '{Time.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE `Token` = '{Session}'");
        }
        /// <summary>
        /// Функция проверки активных подключений
        /// </summary>
        /// <param name="TimeOfCheck">Время проверки</param>
        public static void CheckActiveSessions(DateTime TimeOfCheck)
        {
            DataTable notConnected = DB.MakeQuery($"SELECT `WorkerId` FROM `sessions` WHERE TIMEDIFF(now(), `LastUpdate`) > TIME('{DisconnectTime}')");

            foreach(DataRow row in notConnected.Rows )
            {
                string WorkerId = row[0].ToString();
                string status = GetStatus(WorkerId, TimeOfCheck).Code;

                Logger.Log.Debug(WorkerId);

                if (!IsLongStatus(status) && status != State_Finished)
                {
                    DataTable lastConnect = DB.MakeQuery($"SELECT `LastUpdate` FROM `sessions` WHERE `WorkerId` = '{WorkerId}' ORDER BY `LastUpdate` DESC;");

                    if (lastConnect.Rows.Count == 0) break;

                    if (status != State_Finished || !IsLongStatus(status))
                        LogStatus(WorkerId, State_Finished, DateTime.Parse(lastConnect.Rows[0][0].ToString()));
                    Logger.Log.Debug("Удаление устаревшей сессии");
                }

                DB.ExecuteQuery($"DELETE FROM `sessions` WHERE `WorkerId` = {WorkerId}");
            }
        }
    }
}