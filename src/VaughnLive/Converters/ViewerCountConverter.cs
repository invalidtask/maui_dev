using System.Globalization;

namespace VaughnLive.Converters;

/// <summary>
/// Converts viewer count to a formatted string with abbreviations for large numbers.
/// Examples: 1234 -> "1.2K", 1234567 -> "1.2M"
/// </summary>
public class ViewerCountConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int count)
            return "0";

        return count switch
        {
            >= 1_000_000 => $"{count / 1_000_000.0:F1}M",
            >= 1_000 => $"{count / 1_000.0:F1}K",
            _ => count.ToString("N0")
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
