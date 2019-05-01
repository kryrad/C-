using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Lab01
{
    public class ByteToImageConverter : IValueConverter
    {
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource img = new BitmapImage();
            if (value != null)
            {
                img = ByteToImage(value as byte[]) ;
            }
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return value;          
        }
    }
}
