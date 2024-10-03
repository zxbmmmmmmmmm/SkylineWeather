﻿using System.Globalization;

namespace SkylineWeather.Abstractions.Models;

public struct Location
{
    /// <summary>
    /// An empty location.
    /// </summary>
    public static readonly Location Empty = new Location(0, 0);


    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> struct.
    /// </summary>
    /// <param name="latitude">The latitude.</param>
    /// <param name="longitude">The longitude.</param>
    public Location(double latitude, double longitude)
        : this()
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    private double _latitude;

    /// <summary>
    /// Gets or sets the latitude value of the location.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public double Latitude
    {
        readonly get => _latitude;
        set
        {
            if (value is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(Latitude), "Latitude cannot be less than -90 or greater than 90");
            _latitude = value;
        }
    }

    private double _longitude;

    /// <summary>
    /// Gets or sets the longitude value of the location.
    /// </summary>
    public double Longitude
    {
        readonly get => _longitude;
        set
        {
            value %= 360;
            _longitude = value switch
            {
                > 180 => value - 360,
                < -180 => value + 360,
                _ => value
            };
        }
    }

    /// <summary>
    /// Checks whether two <see cref="Location"/> values are equal.
    /// </summary>
    public static bool operator ==(Location location1, Location location2)
    {
        return location1.Equals(location2);
    }

    /// <summary>
    /// Checks whether two <see cref="Location"/> values are not equal.
    /// </summary>
    public static bool operator !=(Location location1, Location location2)
    {
        return !location1.Equals(location2);
    }

    public static Location operator +(Location location1, Location location2)
    {
        return new Location(location1.Latitude + location2.Latitude, location1.Longitude + location2.Longitude);
    }

    public static Location operator -(Location location1, Location location2)
    {
        return new Location(location1.Latitude - location2.Latitude, location1.Longitude - location2.Longitude);
    }

    /// <summary>
    /// Checks whether two <see cref="Location"/> values are equal.
    /// </summary>
    public static bool Equals(Location location1, Location location2)
    {
        return location1.Latitude == location2.Latitude && location1.Longitude == location2.Longitude;
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return string.Format(CultureInfo.CurrentUICulture, "{0}, {1}", Latitude, Longitude);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is Location location && Equals(this, location);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
}