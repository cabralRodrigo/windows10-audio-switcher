using AudioSwitcher.Util;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioSwitcher.Converters
{
    [ValueConversion(typeof(StateType), typeof(string))]
    public class StateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StateType state))
                return null;

            switch (state)
            {
                case StateType.Mono:
                    return "..\\Assets\\icon-mono.ico";
                case StateType.Stereo:
                    return "..\\Assets\\icon-stereo.ico";
                case StateType.Unknown:
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}