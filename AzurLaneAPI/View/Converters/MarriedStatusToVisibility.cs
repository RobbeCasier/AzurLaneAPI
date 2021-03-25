using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AzurLaneAPI.View.Converters
{
    class MarriedStatusToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool married = (bool)value;
            bool mayshow;
            bool.TryParse(parameter.ToString(), out mayshow);
            if (married && mayshow)
                return Visibility.Visible;
            else if (!married && !mayshow)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
