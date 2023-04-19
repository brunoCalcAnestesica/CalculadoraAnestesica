using System;
using System.Globalization;
using Xamarin.Forms;

namespace CalculadoraAnestesica.Converters
{
    public class BoolToIconConverter : IValueConverter
    {
        private string IconNotFavorite = "start_icon.png";
        private string IconFavorite = "favorite_icon.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return IconFavorite;

            return IconNotFavorite;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = (FileImageSource)value;
            return IconFavorite.Equals(image.File.ToString());
        }
    }
}

