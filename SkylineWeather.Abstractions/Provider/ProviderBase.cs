﻿namespace SkylineWeather.Abstractions.Provider;

public abstract class ProviderBase
{
    public abstract string Name { get; }
    public abstract string Id { get; }
}