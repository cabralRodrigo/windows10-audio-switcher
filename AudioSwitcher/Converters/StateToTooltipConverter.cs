using AudioSwitcher.Util;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioSwitcher.Converters
{
    public class StateToTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is StateType state))
                return null;

            var tooltip = "Audio Switcher" + Environment.NewLine + Environment.NewLine + "Estado Atual: ";

            switch (state)
            {
                case StateType.Mono:
                    tooltip += "Mono";
                    break;
                case StateType.Stereo:
                    tooltip += "Stereo";
                    break;
                case StateType.Unknown:
                default:
                    tooltip += "Desconhecido";
                    break;
            }

            return tooltip;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}