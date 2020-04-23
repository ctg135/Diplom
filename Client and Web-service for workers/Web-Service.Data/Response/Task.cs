namespace Web_Service.Data.Response
{
    /// <summary>
    /// Класс задания
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Номер задания
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Имя сотрудника, установившего задачу
        /// </summary>
        public string SetterWorkerName { get; set; }
        /// <summary>
        /// Дата и время создания
        /// </summary>
        public string Created { get; set; }
        /// <summary>
        /// Дата и время завершения
        /// </summary>
        public string Finished { get; set; }
    }
}
