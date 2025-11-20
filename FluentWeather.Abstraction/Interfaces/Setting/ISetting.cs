using System;

namespace FluentWeather.Abstraction.Interfaces.Setting;

public interface ISetting
{
    public Enum Settings { get; }
    public string Id { get; }
}