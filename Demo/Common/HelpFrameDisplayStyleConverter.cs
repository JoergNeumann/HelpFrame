using Neumann.HelpFrame;
using System;
using Windows.UI.Xaml.Data;

namespace Demo.Common
{
    public class HelpFrameDisplayStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var mode = Enum.Parse(typeof(HelpFrameDisplayStyle), value.ToString());
            return System.Convert.ToInt32(mode);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                return Enum.Parse(typeof(HelpFrameDisplayStyle), value.ToString());
            }
            return HelpFrameDisplayStyle.Classic;
        }
    }
}
