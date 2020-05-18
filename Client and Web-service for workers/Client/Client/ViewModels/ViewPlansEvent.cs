using System;
using System.Collections.Generic;
using Client.DataModels;

namespace Client.ViewModels
{
    /// <summary>
    /// Делегат события для просмотра задач
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="args">Аргументы события</param>
    delegate void ViewPlansEvent(object sender, ViewPlansEventArgs args);
    /// <summary>
    /// Класс аргумента для события <c>ViewPlansEvent</c>
    /// </summary>
    public class ViewPlansEventArgs : EventArgs
    {
        /// <summary>
        /// Список планов для просмотра
        /// </summary>
        public List<Plan> Plans { get; private set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Plans">Список планов для отображения</param>
        public ViewPlansEventArgs(List<Plan> Plans)
        {
            this.Plans = Plans;
        }
    }
}
