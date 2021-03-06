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
    class SubmarineStatToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                if (parameter.ToString().Equals("true"))
                {
                    if (value.ToString().Equals("Submarine"))
                        return Visibility.Collapsed;
                    return Visibility.Visible;
                }
            }
            if (value.ToString().Equals("Submarine"))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
