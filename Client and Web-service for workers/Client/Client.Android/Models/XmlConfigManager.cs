using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Client.Models;
using System.Xml;
using System.Xml.Linq;

namespace Client.Droid.Models
{
    class XmlConfigManager : IConfigManager
    {
        #region Constants
        /// <summary>
        /// Константа для обозначения типа директории для конфигурации
        /// </summary>
        private const string DirectoryConfig = "Config";
        /// <summary>
        /// Имя файла с конфигурацией
        /// </summary>
        private const string ConfigFileName = "config.xml";
        /// <summary>
        /// Имя коренного узла
        /// </summary>
        private const string RootNode = "config";
        #endregion

        /// <summary>
        /// Словарь с временными значениями
        /// </summary>
        private Dictionary<string, string> TempValues { get; set; }
        public XmlConfigManager()
        {
            TempValues = new Dictionary<string, string>();

            var configFile = GetConfigsFilePath(ConfigFileName);

            if (File.Exists(configFile))
            {
                // Читаем существующий
                LoadConfigFromFile(configFile);
            }
            else
            {
                // Создаём новый
                CreateConfigFile(configFile);
            }
        }
        /// <summary>
        /// Функция для создания шаблонного конфигурационного файла
        /// </summary>
        /// <param name="Name">Полный путь конфигурационного файла</param>
        private void CreateConfigFile(string Name)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);

            var root = doc.CreateElement(RootNode);
            doc.AppendChild(root);

            var server = doc.CreateElement("Server");
            server.InnerText = "http://192.168.0.2/api/";
            root.AppendChild(server);

            doc.Save(Name);
        }
        /// <summary>
        /// Функция загрузки данных из конфигурационного файла
        /// </summary>
        /// <param name="Name">Полынй путь до конфигурационного файла</param>
        private void LoadConfigFromFile(string Name)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(Name);

            XmlNode root = doc.DocumentElement;

            foreach (XmlNode node in root)
            {
                TempValues.Add(node.LocalName, node.InnerText);
            }
        }

        /// <summary>
        /// Возвращает директорию для файлов по типу директории
        /// </summary>
        /// <param name="DirectoryType">Тип директории</param>
        /// <returns>Путь до директории</returns>
        private string GetFilesDirectory(string DirectoryType)
        {
            return Application.Context.GetExternalFilesDir(DirectoryType).AbsolutePath;
        }
        /// <summary>
        /// Возвращает путь до конфигурационного файла
        /// </summary>
        /// <param name="File">Имя конфигурационного файла</param>
        /// <returns>Путь конфигурационнного файла</returns>
        private string GetConfigsFilePath(string File)
        {
            return Path.Combine(GetFilesDirectory(DirectoryConfig), File);
        }



        #region Implementation of interface IConfigManager
        /// <summary>
        /// Выбор элемента из конфигурации
        /// </summary>
        /// <param name="Item">Элемент конфигурации</param>
        /// <returns>Значение элемента</returns>
        public async Task<string> GetItem(string Item)
        {
            if (TempValues.ContainsKey(Item))
            {
                return await Task.FromResult(TempValues[Item]);
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(GetConfigsFilePath(ConfigFileName));

                var root = doc.DocumentElement;

                var result = root.SelectNodes(Item);

                if (result.Count > 0)
                {
                    var res = result[0].InnerText;
                    TempValues.Add(Item, res);

                    return await Task.FromResult(res);
                }
                else
                {
                    return await Task.FromResult(string.Empty);
                }
            }
        }
        /// <summary>
        /// Установка значения в конфигурацию
        /// </summary>
        /// <param name="Item">Элемент</param>
        /// <param name="Value">Значение</param>
        /// <returns>Task</returns>
        public async Task SetItem(string Item, string Value)
        {
            if (TempValues.ContainsKey(Item))
            {
                TempValues[Item] = Value;
            }
            else
            {
                TempValues.Add(Item, Value);
            }

            var configFile = GetConfigsFilePath(ConfigFileName);

            XmlDocument doc = new XmlDocument();
            doc.Load(configFile);
            var root = doc.DocumentElement;

            var item = root.SelectSingleNode(Item);

            if (item != null)
            {
                item.InnerText = Value;
            }
            else
            {
                item = doc.CreateElement(Item);
                item.InnerText = Value;

                root.AppendChild(item);
            }

            doc.Save(configFile);
        }
        #endregion
    }
}