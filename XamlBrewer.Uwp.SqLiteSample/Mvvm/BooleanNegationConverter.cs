namespace Mvvm
{
    using System;
    using Windows.UI.Xaml.Data;

    internal class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Boolean b = (Boolean)value;
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}