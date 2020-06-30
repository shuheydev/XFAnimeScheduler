using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFAnimeScheduler.Extensions;

namespace XFAnimeScheduler.Converters
{
    public class ImageSourceConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string embeddedResourcePath = (string)value;

            var imageSource = ImageSource.FromResource(embeddedResourcePath, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
