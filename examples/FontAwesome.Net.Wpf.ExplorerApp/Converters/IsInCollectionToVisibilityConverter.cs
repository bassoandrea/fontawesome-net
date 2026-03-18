using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FontAwesome.Net.Wpf.ExplorerApp.Converters;

public class IsInCollectionToVisibilityConverter
    : MarkupExtension, IValueConverter
{
    public Visibility TrueValue { get; set; } = Visibility.Visible;
    public Visibility FalseValue { get; set; } = Visibility.Collapsed;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ICollection collection)
            return Binding.DoNothing;

        foreach (var item in collection)
        {
            if(item?.Equals(parameter) ?? parameter is null)
                return TrueValue;
        }

        return FalseValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value?.Equals(TrueValue) ?? false) ? parameter : Binding.DoNothing;

    }
}
