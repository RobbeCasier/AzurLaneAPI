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
    class RarityToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rarity = value.ToString();
            rarity = rarity.ToLower();
            if (rarity.Equals("decisive"))
                rarity = "ultra rare";
            else if (rarity.Equals("priority"))
                rarity = "super rare";
            else if (rarity.Equals("normal"))
                rarity = "rare";
            string path = $"/Resources/Frames/{rarity}_bg.png";
            BitmapImage bim = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            return bim;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
