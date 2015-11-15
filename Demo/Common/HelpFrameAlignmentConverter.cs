using Neumann.HelpFrame;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Demo.Common
{
    public class HelpFrameAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                return Enum.Parse(typeof(HelpFrameAlignment), ((ComboBoxItem)value).Content.ToString());
            }
            return HelpFrameAlignment.TopLeft;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}
