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
        private const string ConfigFileName = "config.txt";
        #endregion

        /// <summary>
        /// Словарь с временными значениями
        /// </summary>
        private Dictionary<string, string> TempValues { get; set; }
        public XmlConfigManager()
        {
            TempValues = new Dictionary<string, string>();

            // Проверка на существование файла конфигурации
            // Если есть:
            //      Считать из него все настройки во временный словарь
            // Если нет:
            //      Создать новый по шаблону - значения по умолчанию?

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
        /// <returns>Путь конфигурационнного файла</returns>
        private string GetConfigFilePath()
        {
            return Path.Combine(GetFilesDirectory(DirectoryConfig), ConfigFileName);
        }



        #region Implementation of interface IConfigManager
        /// <summary>
        /// Выбор элемента из конфигурации
        /// </summary>
        /// <param name="Item">Элемент конфигурации</param>
        /// <returns>Значение элемента</returns>
        public Task<string> GetItem(string Item)
        {

            // Как-то всадить асинхронность
            // Проверка ( есть ли в словаре ):
            // Если есть -
            //   вернуть из словрая
            // Если нету -
            //   проверить в конфиге?
            //   вернуть пустое значение

            return null;
        }
        /// <summary>
        /// Установка значения в конфигурацию
        /// </summary>
        /// <param name="Item">Элемент</param>
        /// <param name="Value">Значение</param>
        /// <returns>Task</returns>
        public Task SetItem(string Item, string Value)
        {

            // Как-то всадить асинхронность
            // Проверка ( есть ли в словаре ):
            // Если есть -
            //   вернуть из словрая
            // Если нету - 
            //   записать в конфиг
            //   записать в словарь

            return null;
        }
        #endregion
    }
}