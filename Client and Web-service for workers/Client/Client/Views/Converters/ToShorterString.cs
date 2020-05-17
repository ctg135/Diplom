using System;
using System.Globalization;
using Xamarin.Forms;

namespace Client.Views.Converters
{
    /// <summary>
    /// Преобразует строку длинной более указанного параметра в строку длинной этого параметра + ... на конце
    /// </summary>
    public class ToShorterString : IValueConverter
    {
        /// <summary>
        /// Конвертация в короткую строку по параметру
        /// </summary>
        /// <param name="value">Строка для конвертации</param>
        /// <param name="targetType">Цель применения</param>
        /// <param name="parameter">Число, которое обозначает максимальную длинну текста</param>
        /// <param name="culture">Культура информации</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int max = int.Parse(parameter.ToString());
            string res = value as string;
            if (res.Length > max)
            {
                res = res.Remove(max);
                res += "...";
            }
            return res;
        }
        /// <summary>
        /// Конвертация в короткую строку по параметру
        /// </summary>
        /// <param name="value">Строка для конвертации</param>
        /// <param name="targetType">Цель применения</param>
        /// <param name="parameter">Число, которое обозначает максимальную длинну текста</param>
        /// <param name="culture">Культура информации</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
