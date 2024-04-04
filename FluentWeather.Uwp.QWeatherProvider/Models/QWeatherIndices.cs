using System;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.QWeatherProvider.Models;

public sealed class QWeatherIndices : IndicesBase
{
    public DateTime Date { get; set; }

    public int Type { get; set; }

    public int Level { get; set; }

}