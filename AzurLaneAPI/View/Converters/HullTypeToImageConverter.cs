using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AzurLaneAPI.View.Converters
{
    class HullTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string hull = value.ToString();
                string path = $"/Resources/Icons/{hull}.png";
                BitmapImage bim = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                return bim;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
