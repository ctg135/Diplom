using Client.DataModels;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Models
{
    /// <summary>
    /// Результаты авторизации
    /// </summary>
    public enum AuthorizationResult
    {
        Ok,
        Error
    }
    /// <summary>
    /// Интерфейс для получения данных от сервера
    /// </summary>
    public interface IClientModel
    {
        /// <summary>
        /// Установка сессии по паролю и логину
        /// </summary>
        /// <param name="Login">Логин клиента</param>
        /// <param name="Password">Пароль клиента</param>
        /// <returns>Резуьтат аутентфикации</returns>
        Task<AuthorizationResult> Authorization(string Login, string Password);
        /// <summary>
        /// Авторизация по сессии
        /// </summary>
        /// <returns>Резуьтат аутентфикации</returns>
        Task<AuthorizationResult> Authorization();
        /// <summary>
        /// Сессия пользователя
        /// </summary>
        string Session { get; set; }
        /// <summary>
        /// Сервер с требуемым api
        /// </summary>
        string Server { get; set; }
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
        /// <summary>
        /// Получение планов сотрудника по датам
        /// </summary>
        /// <param name="Start">Начальная дата</param>
        /// <param name="End">Конечная дата</param>
        /// <param name="Filter">Фильтр планов</param>
        /// <returns></returns>
        Task<List<Plan>> GetPlans(DateTime Start, DateTime End, PlanTypes[] Filter);
        /// <summary>
        /// Получение плана на сегодня
        /// </summary>
        /// <returns></returns>
        Task<Plan> GetTodayPlan();
        /// <summary>
        /// Получение списка планов
        /// </summary>
        /// <returns></returns>
        Task<List<PlanType>> GetPlanTypes();
        /// <summary>
        /// Получение стадий задач
        /// </summary>
        /// <returns></returns>
        Task<List<TaskStage>> GetTaskStages();
        /// <summary>
        /// Полуение списка задач по фильтру
        /// </summary>
        /// <param name="Filter">Фильтр задач по стадии</param>
        /// <returns></returns>
        Task<List<Tasks.Task>> GetTasks(DateTime Created, TaskStages[] Filter);
        /// <summary>
        /// Принятие задачи
        /// </summary>
        /// <returns></returns>
        Task AcceptTask(string TaskId);
        /// <summary>
        /// Завершение задачи
        /// </summary>
        /// <returns></returns>
        Task CompleteTask(string TaskId);
    }
    /// <summary>
    /// Нумерация стадий задач
    /// </summary>
    public enum TaskStages
    {
        /// <summary>
        /// Задача не принята
        /// </summary>
        NotAccepted,
        /// <summary>
        /// Задача в процессе выполнения
        /// </summary>
        Processing,
        /// <summary>
        /// Задача завершена
        /// </summary>
        Completed
    }
    /// <summary>
    /// Нумерация типов планов
    /// </summary>
    public enum PlanTypes
    {
        /// <summary>
        /// Тип рабочего дня
        /// </summary>
        Working,
        /// <summary>
        /// Тип отпуска
        /// </summary>
        Holiday,
        /// <summary>
        /// Тип больничного
        /// </summary>
        Hospital,
        /// <summary>
        /// Тип выходного
        /// </summary>
        DayOff
    }
}
