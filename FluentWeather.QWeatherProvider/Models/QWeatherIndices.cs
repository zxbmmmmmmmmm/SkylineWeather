using FluentWeather.Abstraction.Models;
using System;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherIndices : IndicesBase
{
    public DateTime Date { get; set; }

    public int Type { get; set; }

    public int Level { get; set; }

}