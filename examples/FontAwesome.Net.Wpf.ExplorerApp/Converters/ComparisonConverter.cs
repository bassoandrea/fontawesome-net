using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FontAwesome.Net.Wpf.ExplorerApp.Converters;

public class ComparisonConverter
    : MarkupExtension, IValueConverter
{
    public object? TrueValue { get; set; } = true;
    public object? FalseValue { get; set; } = false;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value?.Equals(parameter) ?? (parameter is null)) ? TrueValue : FalseValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value?.Equals(TrueValue) ?? (TrueValue is null)) ? parameter : Binding.DoNothing;
    }
}
