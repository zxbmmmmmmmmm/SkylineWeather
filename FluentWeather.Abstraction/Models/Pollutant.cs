using System.Globalization;

namespace FluentWeather.Abstraction.Models;

public class Pollutant
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public string? Unit { get; set; }
    public double Value { get; set; }

    public string DisplayValue => Value.ToString("0.##", CultureInfo.CurrentCulture);
}
