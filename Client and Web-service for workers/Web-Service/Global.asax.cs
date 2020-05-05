using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {       
        protected void Application_Start()
        {
            // Настройка маршрутизации

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Logger.Log.Info("Сервер запущен!");

            // Настройка подключения к базе данных

            DBClient.DB = new DBWorkerMySql(ReaderConfig.ConnectionStringDB);
            try
            {
                if (DBClient.DB.CheckConnection())
                {
                    Logger.Log.Info("Подключение к базе данных установлено");
                }
            }
            catch(Exception exc)
            {
                Logger.Log.Fatal("Подключение к базе данных отсутствует");
                Logger.Log.Debug(exc, "Ошибка подключения к базе данных:");
            }

            DBClient.LongStatuses = new List<string>(ReaderConfig.LongStatuses);
            DBClient.State_NotState = ReaderConfig.Status_NotStated;
            DBClient.State_Finished = ReaderConfig.Status_Finished;
            DBClient.State_DayOff = ReaderConfig.State_DayOff;
            DBClient.State_Working = ReaderConfig.State_Working;
            DBClient.State_Paused = ReaderConfig.State_Paused;
            DBClient.DisconnectTime = ReaderConfig.DisconnectTime;
            DBClient.LongStatusGraphics = ReaderConfig.GraphicCode_StatusCode;
            DBClient.DayType_Working = ReaderConfig.Day_Working;
            DBClient.DayType_DayOff = ReaderConfig.Day_DayOff;
            DBClient.TaskStage_Accepted = ReaderConfig.Task_Accepted;
            DBClient.TaskStage_NotAccepted = ReaderConfig.Task_NotAccepted;
            DBClient.TaskStage_Completed = ReaderConfig.Task_Completed;
            DBClient.TaskStages = new List<string>();
            DBClient.TaskStages.AddRange(new string[3] { DBClient.TaskStage_Accepted, DBClient.TaskStage_NotAccepted, DBClient.TaskStage_Completed } );

            // Настройка процедуры проверки подключений

            var timer = new Timer(new TimerCallback(OnTimer));
            timer.Change(0, ReaderConfig.PeriodCheckConnection);
        }
        private static void OnTimer(object obj)
        {
            Logger.Log.Info("Проверка подключений");
            DBClient.CheckActiveSessions(DateTime.Now);
            Logger.Log.Info("Проверка подключений завершена");
        }
    }
}
