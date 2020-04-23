namespace Web_Service.Data.Response
{
    /// <summary>
    /// Класс, представляющий статус
    /// </summary>
    public class StatusType
    {
        /// <summary>
        /// Код статуса
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// Название статуса
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Описание статуса
        /// </summary>
        public string Description { get; set; }
    }
}
