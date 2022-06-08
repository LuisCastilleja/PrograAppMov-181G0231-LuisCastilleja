using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using AppMovilPasteleria.Models;
namespace AppMovilPasteleria.Helpers
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (!String.IsNullOrWhiteSpace(value.ToString()))
            {
                return decimal.Parse(value.ToString());
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!String.IsNullOrWhiteSpace(value.ToString()))
            {
                return decimal.Parse(value.ToString());
            }
            else
            {
                return 0;
            }
        }
    }
}
