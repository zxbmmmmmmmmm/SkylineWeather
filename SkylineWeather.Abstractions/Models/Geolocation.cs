using System.Diagnostics.CodeAnalysis;

namespace SkylineWeather.Abstractions.Models;

public class Geolocation
{
    public required string Name { get; set; }
    public required Location Location { get; set; }
    public string? AdmDistrict { get; set; }
    public string? AdmDistrict2 { get; set; }
    public string? Region { get; set; }
    public override string ToString()
    {
        return $"{Name} {Location}";
    }

    [SetsRequiredMembers]
    public Geolocation(string name, double lat, double lon)
    {
        this.Name = name;
        this.Location = new Location(lat, lon);
    }

    [SetsRequiredMembers]
    public Geolocation(string name,Location location)
    {
        this.Name = name;
        this.Location = location;
    }
    public Geolocation()
    {
        
    }
}