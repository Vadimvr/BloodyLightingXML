using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using ColorPicker;
using System.Windows.Media;
using ColorPicker.Models;

namespace mouse_lighting.Resources.Converters
{
    public class ColorStageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            ColorState colorStage = new ColorState();
            colorStage.SetARGB(color.A, color.R, color.G, color.B);

            return colorStage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ColorState colorStage = (ColorState)value;

            return Color.FromRgb((byte)colorStage.RGB_R, (byte)colorStage.RGB_G, (byte)colorStage.RGB_B);
        }
    }
}
