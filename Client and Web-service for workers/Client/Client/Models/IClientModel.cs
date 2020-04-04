using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    /// <summary>
    /// Интерфейс для получения данных от сервера
    /// </summary>
    interface IClientModel
    {
        /// <summary>
        /// Производит авторизацию
        /// </summary>
        IAuthorizationModel Authorization { get; }
    }
}
