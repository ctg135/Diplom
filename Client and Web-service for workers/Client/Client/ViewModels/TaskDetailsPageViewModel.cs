using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Client.ViewModels.Tasks;

namespace Client.ViewModels
{
    /// <summary>
    /// Модель для просмотра данных о задаче
    /// </summary>
    class TaskDetailsPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Клиент для взаимодействия с сервером
        /// </summary>
        private IClientModel Client { get; set; }
        /// <summary>
        /// Значение для просмотра
        /// </summary>
        public Task Item { get; set; }
        /// <summary>
        /// Команда нажатой кнопки 
        /// </summary>
        /// <details>
        /// Команда может быть <c>AcceptTask</c> или <c></c>
        /// </details>
        public ICommand ButtonCommand { get; set; }
        /// <summary>
        /// Текст кнопки
        /// </summary>
        public string ButtonText { get; set; }
        /// <summary>
        /// Опция видимости кнопки
        /// </summary>
        public bool ButtonActivity { get; set; }
        /// <summary>
        /// Событие выхода из страницы
        /// </summary>
        public event EventHandler Exit;
        /// <summary>
        /// Конструктор модели
        /// </summary>
        /// <param name="Item">Элемент для просмотра</param>
        public TaskDetailsPageViewModel(Task Item)
        {
            this.Client = CommonServiceLocator.ServiceLocator.Current.GetInstance<IClientModel>();
            this.Client.Session = Globals.Config.GetItem("Session").Result;
            this.Client.Server = Globals.Config.GetItem("Server").Result;
            this.Item = Item;
            ButtonActivity = new bool();
            ButtonText = string.Empty;
            if (Item.Stage == Globals.TaskStages["1"]) // Если задача непринята
            {
                ButtonText = "Принять задачу";
                if (Globals.WorkerStatus.Code == "2" || Globals.WorkerStatus.Code == "3")
                {
                    ButtonActivity = true;
                    ButtonCommand = new Command(AcceptTask);
                }
                else
                {
                    ButtonActivity = false;
                }
            }
            else if (Item.Stage == Globals.TaskStages["2"]) // Если задача выполняется
            {
                ButtonText = "Завершить задачу";
                if (Globals.WorkerStatus.Code == "2" || Globals.WorkerStatus.Code == "3")
                {
                    ButtonActivity = true;
                    ButtonCommand = new Command(CompleteTask);
                }
                else
                {
                    ButtonActivity = false;
                }
            }
            else if (Item.Stage == Globals.TaskStages["3"]) // Если задача завершена
            {
                ButtonActivity = false;
                ButtonText = $"Завершена {Item.DateFinished}";
            }
            else
            {
                throw new Exception($"Необработанная стадия '{Item.Stage}' задачи '{Item.Id}'");
            }
        }
        /// <summary>
        /// Команда принятия задачи
        /// </summary>
        /// <param name="param"></param>
        private async void AcceptTask(object param)
        {            
            if (Globals.WorkerStatus.Code == "2" || Globals.WorkerStatus.Code == "3")
            {
                try
                {
                    await Client.AcceptTask(Item.Id);
                    Exit(this, new EventArgs());
                }
                catch (Exception exc)
                {
                    await FatalError(exc.Message);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Задания можно выполнять тоько на работе!", "Ок");
            }
        }
        /// <summary>
        /// Команда завершения задачи
        /// </summary>
        /// <param name="param"></param>
        private async void CompleteTask(object param)
        {
            try
            {
                await Client.CompleteTask(Item.Id);
                Exit(this, new EventArgs());
            }
            catch (Exception exc)
            {
                await FatalError(exc.Message);
            }
        }
    }
}
