using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.DataModels;
using Client.ViewModels;

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
        Task<Worker1> GetWorkerInfo();
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
        /// Проверяет, ошибка является ошибкой клиента для устанвоки статуса
        /// </summary>
        /// <param name="ErrorMessage">Сообщение об ошибке</param>
        /// <returns>true, если является ошибкой клиента</returns>
        Task<bool> IsSetStatusClientError(string ErrorMessage);
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
        Task<Plan1> GetTodayPlan();
        /// <summary>
        /// Функция для проверки подключения к серверу
        /// </summary>
        /// <returns></returns>
        Task<AuthorizationResult> CheckConnect();
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
        Task<List<Tasks.Task>> GetTasks(TaskStages[] Filter);
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
}
