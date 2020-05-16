using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Client.DataModels;
using Client.Models;

namespace Client.Droid.Models
{
    public class XmlPlanLoader : IPlanLoader
    {
        #region NodeNames
        /// <summary>
        /// Имя корнего элемента документа
        /// </summary>
        private const string RootNode = "data";
        /// <summary>
        /// Имя элемента с планами
        /// </summary>
        private const string PlanNode = "plan";
        /// <summary>
        /// Имя элемента с датой
        /// </summary>
        private const string DateSetNode = "date";
        /// <summary>
        /// Имя элемента с типом плана
        /// </summary>
        private const string TypePlanNode = "type";
        /// <summary>
        /// Имя элемента со временем начала дня
        /// </summary>
        private const string StartDayNode = "start";
        /// <summary>
        /// Имя элемента с конечной датой
        /// </summary>
        private const string EndDayNode = "end";
        #endregion
        /// <summary>
        /// Имя файла с конфигурацией
        /// </summary>
        private const string PlansFile = "PlanList.xml";
        /// <summary>
        /// Каталог файлов для списка планов
        /// </summary>
        private string PlansFolder { get; set; }
        /// <summary>
        /// Путь до <c>PlanList.xml</c> файла с планами
        /// </summary>
        private string PlansFileFolder  { get; set; }
        public XmlPlanLoader()
        {
            PlansFolder = Application.Context.GetExternalFilesDir("Plans").AbsolutePath;
            PlansFileFolder = Path.Combine(PlansFolder, PlansFile);

            if (!File.Exists(PlansFileFolder))
            {
                CreateEmptyPlansFile();
            }
        }
        /// <summary>
        /// Создание пустого файла с планами
        /// </summary>
        public void CreateEmptyPlansFile() 
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);

            var root = doc.CreateElement(RootNode);
            doc.AppendChild(root);

            doc.Save(PlansFileFolder);
        }
        #region Implementation of IPlanLoader
        public async Task ClearPlans()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(PlansFileFolder);

            var root = doc.DocumentElement;

            if (root.HasChildNodes)
            {
                root.RemoveAll();
                doc.Save(PlansFileFolder);
            }

            await Task.CompletedTask;
        }

        public async Task<List<Plan1>> GetPlans()
        {
            List<Plan1> plans = new List<Plan1>();

            XmlDocument doc = new XmlDocument();
            doc.Load(PlansFileFolder);

            foreach (XmlNode planNode in doc.DocumentElement.SelectNodes(PlanNode))
            {
                plans.Add(new Plan1()
                {
                    DateSet  = planNode.SelectSingleNode(DateSetNode).InnerText,
                    TypePlan = planNode.SelectSingleNode(TypePlanNode).InnerText,
                    StartDay = planNode.SelectSingleNode(StartDayNode).InnerText,
                    EndDay   = planNode.SelectSingleNode(EndDayNode).InnerText
                });
            }

            return await Task.FromResult(plans);
        }

        public async Task SetPlans(List<Plan1> Plans)
        {
            if (Plans.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PlansFileFolder);

                var rootNode = doc.DocumentElement;

                if (rootNode.HasChildNodes)
                {
                    rootNode.RemoveAll();
                }

                foreach (var plan in Plans)
                {
                    var planEl = doc.CreateElement(PlanNode);

                    var temp = doc.CreateElement(DateSetNode);
                    temp.InnerText = plan.DateSet;
                    planEl.AppendChild(temp);

                    temp = doc.CreateElement(TypePlanNode);
                    temp.InnerText = plan.TypePlan;
                    planEl.AppendChild(temp);

                    temp = doc.CreateElement(StartDayNode);
                    temp.InnerText = plan.StartDay;
                    planEl.AppendChild(temp);

                    temp = doc.CreateElement(EndDayNode);
                    temp.InnerText = plan.EndDay;
                    planEl.AppendChild(temp);

                    rootNode.AppendChild(planEl);
                }

                doc.Save(PlansFileFolder);
            }
            await Task.CompletedTask;
        }
        #endregion
    }
}