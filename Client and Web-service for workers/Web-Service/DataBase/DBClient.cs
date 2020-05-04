using System;
using System.Collections.Generic;
using System.Data;
using Web_Service.Loggers;
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


        #region Constatnts        


        /// <summary>
        /// Список длительных статусов
        /// </summary>
        public static List<string> LongStatuses { get; set; }
        /// <summary>
        /// Коды длинных статусов { Тип графика - Код статуса }
        /// </summary>
        public static Dictionary<string, string> LongStatusGraphics { get; set; }
        /// <summary>
        /// Номер рабочего дня
        /// </summary>
        public static string DayType_Working { get; set; }
        /// <summary>
        /// Номер выходного дня
        /// </summary>
        public static string DayType_DayOff { get; set; }
        /// <summary>
        /// Код неустановленного статуса
        /// </summary>
        public static string State_NotState { get; set; }
        /// <summary>
        /// Код статуса завершённой работы
        /// </summary>
        public static string State_Finished { get; set; }
        /// <summary>
        /// Статус выходного дня
        /// </summary>
        public static string State_DayOff { get; set; } 
        /// <summary>
        /// Статус процесса работы
        /// </summary>
        public static string State_Working { get; set; }
        /// <summary>
        /// Значение стадии завершенной задачи
        /// </summary>
        public static string TaskStage_Accepted { get; set; }
        /// <summary>
        /// Значение стадии непринятой к выполнению задачи
        /// </summary>
        public static string TaskStage_NotAccepted { get; set; }
        /// <summary>
        /// Значение завершенной стадии задачи
        /// </summary>
        public static string TaskStage_Completed { get; set; }
        /// <summary>
        /// Время отключения неработающей сессии
        /// </summary>
        public static string DisconnectTime { get; set; }


        #endregion


        /// <summary>
        /// Функция записи сессии в базу данных
        /// </summary>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static void InsertSession(string WorkerId, string SessionHash, string ClientInfo, DateTime DateCreation) 
        => DB.Insert("sessions", new Dictionary<string, string>() 
        {
            { "WorkerId",   WorkerId },
            { "Token",      SessionHash },
            { "ClientInfo", ClientInfo },
            { "Created",    DateCreation.ToString("yyyy-MM-dd HH:mm:ss") },
            { "LastUpdate", DateCreation.ToString("yyyy-MM-dd HH:mm:ss") }
        });
        /// <summary>
        /// Фукнция поиска сессии по Id работника и его клиенту
        /// </summary>
        /// <param name="WorkerId"></param>
        /// <param name="ClientInfo"></param>
        /// <returns>Хэш сессии в результат или <c>string.Empty</c>, если не найдена</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static string SearchSession(string WorkerId, string ClientInfo)
        {
            var table = DB.MakeQuery($"SELECT `Token` FROM `sessions` WHERE `WorkerId` = '{WorkerId}' AND `ClientInfo` = '{ClientInfo}';");
            return table.Rows.Count == 0 ? string.Empty : table.Rows[0][0].ToString();
        }
        /// <summary>
        /// Возаращает Id работника по данным авторизации
        /// <br><c>string.Empty</c> - в случае ненахождения такого</br>
        /// </summary>
        /// <param name="Login">Логин работника</param>
        /// <param name="Password">Пароль работника</param>
        /// <returns>Id работника или string.Empty, если не найден</returns>
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

            UpdateSession(Session, DateTime.Now);

            return id;
        }
        /// <summary>
        /// Возвращает номер работника, выполняющий <c>TaskId</c>
        /// </summary>
        /// <param name="TaskId">Номер задачи, на которую назанчен работник</param>
        /// <returns>Номер работника</returns>
        public static string GetWorkerIdFromTask(string TaskId)
        {
            string WorkerId = string.Empty;

            DataTable data = DB.MakeQuery($"SELECT `SetWorker` FROM `tasks` WHERE `Id` = '{TaskId}'");
            if (data.Rows.Count == 1)
            {
                WorkerId = data.Rows[0][0].ToString();
            }

            return WorkerId;
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
        public static Data.Response.WorkerInfo GetWorker(string WorkerId)
        {
            var data = DB.MakeQuery($"SELECT `Name`, `Surname`, `Patronymic`, `Position`, `Department` FROM `workers` WHERE `Id` = '{WorkerId}'");

            if(data.Rows.Count != 1)
            {
                throw new Exception("Работник не найден");
            }

            var worker = new Data.Response.WorkerInfo()
            {
                Name        = data.Rows[0]["Name"].ToString(),
                Surname     = data.Rows[0]["Surname"].ToString(),
                Patronymic  = data.Rows[0]["Patronymic"].ToString(),
                Position    = data.Rows[0]["Position"].ToString(),
                Department  = data.Rows[0]["Department"].ToString()
            };

            data = DB.MakeQuery($"SELECT `Name` FROM `department` WHERE `Id` = '{worker.Department}'");

            if (data.Rows.Count == 1)
            {
                worker.Department = data.Rows[0][0].ToString();
            }

            return worker;
        }
        /// <summary>
        /// Функция выбор всех видов статуса работников
        /// </summary>
        /// <returns>Все статусы</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static IEnumerable<Data.Response.StatusType> GetStatusTypes()
        {
            List<Data.Response.StatusType> statuses = new List<Data.Response.StatusType>();

            DataTable data = DB.SelectTable("statuses");

            foreach(DataRow row in data.Rows)
            {
                statuses.Add(new Data.Response.StatusType()
                {
                    StatusCode  = row["Code"].ToString(),
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
        /// <param name="CheckDate">Дата проверки</param>
        /// <returns>Статус работника</returns>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static Data.Response.Status GetStatus(string WorkerId, DateTime CheckDate)
        {
            DataTable data = DB.MakeQuery(
                "SELECT `StatusCode`, `SetDate`, `SetTime` " +
                "FROM `statuslogs` " +
                $"WHERE `WorkerId` = '{WorkerId}' AND `SetDate` = '{CheckDate:yyyy-MM-dd}' " +
                "ORDER BY `SetDate` DESC, `SetTime` DESC " +
                "LIMIT 1;"
            );

            if (data.Rows.Count == 1) // Если были логи за сегодня, то возвращаем последний
            {
                DateTime setted = DateTime.Parse(data.Rows[0]["SetDate"].ToString());
                setted.Add(TimeSpan.Parse(DateTime.Parse(data.Rows[0]["SetDate"].ToString()).ToString("HH:mm:ss")));
                return new Data.Response.Status() { StatusCode = data.Rows[0]["StatusCode"].ToString(), Updated = setted.ToString()};
            }
            else
            {   
                data = DB.MakeQuery($"SELECT `DayType` FROM `plans` WHERE `WorkerId` = '{WorkerId}' AND `Date` = '{CheckDate:yyyy-MM-dd}'");

                if (data.Rows.Count == 1) // Если есть график, значит день ( рабочий | отпуск | болька )
                {
                    var dayType = data.Rows[0]["DayType"].ToString();
                    if (dayType == DayType_Working) // Если рабочий день
                    {
                        return new Data.Response.Status() { StatusCode = State_NotState, Updated = CheckDate.ToString() };
                    }
                    else if (LongStatusGraphics.ContainsKey(dayType)) // Если длительный статус
                    {
                        return new Data.Response.Status() { StatusCode = LongStatusGraphics[dayType], Updated = CheckDate.ToString() };
                    }
                    else // В ином случае
                    {
                        throw new Exception("Несуществующий тип статуса в базе данных");
                    }
                }
                else if (data.Rows.Count == 0) // Графика нету
                {
                    return new Data.Response.Status() { StatusCode = LongStatusGraphics[DayType_DayOff], Updated = CheckDate.ToString() };
                }
                else
                {
                    throw new Exception($"Наложение '{data.Rows.Count}' графиков у работника {WorkerId} на {CheckDate} число");
                }
            }
        }
        /// <summary>
        /// Функция, которая определяет, может ли статус быть обновлён
        /// </summary>
        /// <param name="StatusCode">Статус для проверки</param>
        /// <returns>true, если статус можно обновить</returns>
        public static bool IsStatusCanBeUpdated(string StatusCode)
        {
            if (IsLongStatus(StatusCode))
            {
                Logger.StatusLog.Trace("Длинный статус не может быть изменён");
                return false;
            }
            else if (StatusCode == State_Finished)
            {
                Logger.StatusLog.Trace("Статус завершенного дня не может быть изменён");
                return false;
            }
            else if (StatusCode == State_DayOff)
            {
                Logger.StatusLog.Trace("Статус выходного не может быть изменён");
                return false;
            }
            else
            {
                Logger.StatusLog.Trace("Статус может быть изменён");
                return true;
            }
        }
        /// <summary>
        /// Функция, определяющая может ли статус быть установленным
        /// </summary>
        /// <param name="StatusCode">Стату для проверки</param>
        /// <returns>true, если возможно установить</returns>
        public static bool IsStatusCanBeSetted (string StatusCode)
        {
            if (IsLongStatus(StatusCode))
            {
                Logger.StatusLog.Trace("Длинный статус был получен");
                return false;
            }
            else if (StatusCode == State_DayOff)
            {
                Logger.StatusLog.Trace("Статус выходного дня получен");
                return false;
            }
            else if (StatusCode == State_NotState)
            {
                Logger.StatusLog.Trace("Статус неустаноленного статуса получен");
                return false;
            }
            else
            {
                Logger.StatusLog.Trace("Статус может быть установлен");
                return true;
            }
        }
        /// <summary>
        /// Указыввает, является ли переданный статус "длительным"
        /// </summary>
        /// <param name="StatusCode">Статус</param>
        /// <returns>Если да, то true</returns>
        public static bool IsLongStatus(string StatusCode) => LongStatuses.Contains(StatusCode);
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
        /// Возвращает типы планов
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Data.Response.PlanType> GetPlanTypes()
        {
            var types = new List<Data.Response.PlanType>();

            var data = DB.SelectTable("daytypes");

            foreach (DataRow row in data.Rows)
            {
                types.Add
                (
                    new Data.Response.PlanType()
                    {
                        PlanCode = row["Id"].ToString(),
                        Title = row["Description"].ToString()
                    }
                );
            }

            return types;
        }
        /// <summary>
        /// Функция для получения планов на количсество дней от указанной даты
        /// </summary>
        /// <param name="WorkerId">Идентификатор работника</param>
        /// <param name="StartDay">Начальынй день</param>
        /// <param name="EndDay">Конечный день</param>
        /// <exception cref="Exception">Ошибка запроса</exception>
        public static IEnumerable<Data.Response.Plan> GetPlans(string WorkerId, DateTime StartDay, DateTime EndDay)
        {
            var plans = new List<Data.Response.Plan>();

            DataTable data = DB.MakeQuery($"SELECT * FROM `plans` WHERE `WorkerId` = '{WorkerId}' " +
                $"AND `Date` >= '{StartDay:yyyy-MM-dd}' " +
                $"AND `Date` <= '{EndDay:yyyy-MM-dd}' ORDER BY `Date` ASC");

            foreach(DataRow row in data.Rows)
            {
                plans.Add(new Data.Response.Plan() 
                {
                    Date     = DateTime.Parse(row["Date"].ToString()).ToString("dd.MM.yyyy"),
                    PlanCode = row["DayType"].ToString(),
                    DayStart = row["StartOfDay"].ToString(),
                    DayEnd   = row["EndOfDay"].ToString()
                });
            }
            return plans;
        }
        /// <summary>
        /// Функция для получения планов на количсество дней от указанной даты
        /// </summary>
        /// <param name="WorkerId">Идентификатор работника</param>
        /// <param name="StartDay">Начальынй день</param>
        /// <param name="EndDay">Конечный день</param>
        /// <param name="DayTypes">Список дней для отбора</param>
        /// <returns></returns>
        public static IEnumerable<Data.Response.Plan> GetPlans(string WorkerId, DateTime StartDay, DateTime EndDay, List<string> DayTypes)
        {
            string PlanList = "(";

            foreach (string daytype in DayTypes)
            {
                PlanList += $"'{daytype}',";
            }
            PlanList = PlanList.Remove(PlanList.Length - 1, 1);
            PlanList += ")";

            var plans = new List<Data.Response.Plan>();

            DataTable data = DB.MakeQuery($"SELECT * FROM `plans` WHERE `WorkerId` = '{WorkerId}' " +
                $"AND `Date` >= '{StartDay:yyyy-MM-dd}' " +
                $"AND `Date` <= '{EndDay:yyyy-MM-dd}' " +
                $"AND `DayType` IN {PlanList}" + 
                $"ORDER BY `Date` ASC");

            foreach (DataRow row in data.Rows)
            {
                plans.Add(new Data.Response.Plan()
                {
                    Date = DateTime.Parse(row["Date"].ToString()).ToString("dd.MM.yyyy"),
                    PlanCode = row["DayType"].ToString(),
                    DayStart = row["StartOfDay"].ToString(),
                    DayEnd = row["EndOfDay"].ToString()
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
            var table = DB.MakeQuery($"SELECT * FROM `sessions` WHERE `Token` = '{Session}'");

            if (table.Rows.Count == 1)
            {
                DB.ExecuteQuery($"UPDATE `sessions` SET `LastUpdate` = '{Time.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE `Token` = '{Session}'");
            }
            else
            {
                throw new Exception("Ошибка обновления");
            }
        }
        /// <summary>
        /// Функция проверки активных подключений
        /// </summary>
        /// <param name="TimeOfCheck">Время проверки</param>
        public static void CheckActiveSessions(DateTime TimeOfCheck)
        {
            if(Logger.Log.IsDebugEnabled)
            {
                string allSessionCount = DB.MakeQuery("SELECT COUNT(*) FROM `sessions`").Rows[0][0].ToString();
                Logger.Log.Debug($"Всего сессий - {allSessionCount}");
            }

            DataTable notConnected = DB.MakeQuery($"SELECT `WorkerId`, `Token` FROM `sessions` WHERE TIMEDIFF(now(), `LastUpdate`) > TIME('{DisconnectTime}')");
            Logger.Log.Debug($"Всего неактивных сессий - {notConnected.Rows.Count}");

            foreach( DataRow row in notConnected.Rows )
            {
                string WorkerId = row[0].ToString();
                string Session  = row[1].ToString();
                string status   = GetStatus(WorkerId, TimeOfCheck).StatusCode;

                Logger.Log.Debug("Для #" + WorkerId + " - устаревшая сессия " + Session);

                DataTable lastConnect = DB.MakeQuery($"SELECT `LastUpdate` FROM `sessions` WHERE `Token` = '{Session}' ORDER BY `LastUpdate` DESC;");

                if (lastConnect.Rows.Count == 0) continue;

                Logger.Log.Debug($"Установленный статус #{WorkerId} - {status}");

                if (status != State_Finished && status != State_NotState && !IsLongStatus(status))
                {
                    try
                    {
                        Logger.Log.Debug("Установка статуса завершенного дня для #" + WorkerId);
                        LogStatus(WorkerId, State_Finished, DateTime.Parse(lastConnect.Rows[0][0].ToString()));
                    }
                    catch (Exception exc)
                    {
                        Logger.Log.Error(exc, "Ошибка установки статуса");
                    }
                }

                Logger.Log.Debug("Удаление сессии...");
                try
                {
                    DB.ExecuteQuery($"DELETE FROM `sessions` WHERE `Token` = '{Session}'");
                }
                catch(Exception exc)
                {
                    Logger.Log.Error(exc, "Ошибка удаления сессии");
                }
            }
        }
        /// <summary>
        /// Возвращает список стадий задач
        /// </summary>
        /// <returns>Список стадий задач</returns>
        public static IEnumerable<Data.Response.TaskStage> GetTaskStages()
        {
            var stages = new List<Data.Response.TaskStage>();

            var data = new DataTable();

            data = DB.SelectTable("taskstage");

            foreach (DataRow row in data.Rows)
            {
                stages.Add(new Data.Response.TaskStage() 
                {
                    Stage = row["Id"].ToString(),
                    Title = row["Description"].ToString()
                });
            }

            return stages;
        }
        /// <summary>
        /// Функция завершения задачи
        /// </summary>
        /// <param name="TaskId">Номер задачи</param>
        /// <param name="NewStage">Новая стадия</param>
        public static void UpdateTaskStage(string TaskId, string NewStage)
        {
            DB.ExecuteQuery($"UPDATE `tasks` SET `Stage` = '{NewStage}' WHERE `Id` = '{TaskId}'");
            if (NewStage == TaskStage_Completed)
            {
                DB.ExecuteQuery($"UPDATE `tasks` SET `Finished` = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}' WHERE `Id` = '{TaskId}'");
            }
        }
        /// <summary>
        /// Функция, которая аолучает значение стадии задачи
        /// </summary>
        /// <param name="TaskId">Номер задачи</param>
        /// <returns></returns>
        public static string GetTaskStage(string TaskId)
        {
            var data = DB.MakeQuery($"SELECT `Stage` FROM `tasks` WHERE `Id` = '{TaskId}'");
            string stage = string.Empty;

            if (data.Rows.Count == 1)
            {
                stage = data.Rows[0][0].ToString();
            }

            return stage;

        }
    }
}