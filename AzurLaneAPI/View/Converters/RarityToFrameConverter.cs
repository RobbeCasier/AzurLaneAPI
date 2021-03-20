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
    class RarityToFrameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rarity = value.ToString();
            rarity = rarity.ToLower();
            string path = $"/Resources/Frames/{rarity}.png";
            BitmapImage bim = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            return bim;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
