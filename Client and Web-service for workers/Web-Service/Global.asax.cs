using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web_Service.DataBase;

namespace Web_Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {       
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Настройка логгера
            Logger.InitLogger();
            Logger.Log.Info("Сервер запущен!");
            // Настройка подключения к базе данных
            DBClient.DB = new DBWorkerMySql(ReaderConfig.ConnectionStringDB);
            if(DBClient.DB.CheckConnection())
            {
                Logger.Log.Info("Подключение к базе данных установлено");
            }
            else
            {
                Logger.Log.Error("Подключение к базе данных отсутствует");
            }
        }

        private void WebApiApplication_Disposed(object sender, EventArgs e)
        {
            Logger.Log.Info("Сервер завершает работу");
        }
    }
}
