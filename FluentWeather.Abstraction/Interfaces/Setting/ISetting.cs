using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentWeather.Abstraction.Interfaces.Setting;

public interface ISetting
{
    public Enum Settings{ get; }
    public string Id { get; }
}