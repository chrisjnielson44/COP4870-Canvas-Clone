using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace COP4870_Canvas_Clone.Converters;

public class BooleanToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Colors.LightGray : Colors.Transparent; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
