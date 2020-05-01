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
            // ��������� �������������

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Logger.Log.Info("������ �������!");

            // ��������� ����������� � ���� ������

            DBClient.DB = new DBWorkerMySql(ReaderConfig.ConnectionStringDB);
            try
            {
                if (DBClient.DB.CheckConnection())
                {
                    Logger.Log.Info("����������� � ���� ������ �����������");
                }
            }
            catch(Exception exc)
            {
                Logger.Log.Fatal("����������� � ���� ������ �����������");
                Logger.Log.Debug(exc, "������ ����������� � ���� ������:");
            }

            DBClient.LongStatuses = new List<string>(ReaderConfig.LongStatuses);
            DBClient.State_NotState = ReaderConfig.Status_NotStated;
            DBClient.State_Finished = ReaderConfig.Status_Finished;
            DBClient.DisconnectTime = ReaderConfig.DisconnectTime;
            DBClient.LongStatusGraphics = ReaderConfig.GraphicCode_StatusCode;
            DBClient.DayType_Working = ReaderConfig.Day_Working;
            DBClient.DayType_DayOff = ReaderConfig.Day_DayOff;

            // ��������� ��������� �������� �����������

            var timer = new Timer(new TimerCallback(OnTimer));
            timer.Change(0, ReaderConfig.PeriodCheckConnection);
        }
        private static void OnTimer(object obj)
        {
            Logger.Log.Info("�������� �����������");
            DBClient.CheckActiveSessions(DateTime.Now);
            Logger.Log.Info("�������� ����������� ���������");
        }
    }
}
