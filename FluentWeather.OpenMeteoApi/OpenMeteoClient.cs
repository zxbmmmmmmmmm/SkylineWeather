using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentWeather.OpenMeteoApi.Models;

namespace FluentWeather.OpenMeteoApi;
/// <summary>
/// These codes are based on open-meteo-dotnet
/// https://github.com/AlienDwarf/open-meteo-dotnet/tree/master
/// </summary>
public class OpenMeteoClient
{
    private readonly string _weatherApiUrl = "https://api.open-meteo.com/v1/forecast";
    private readonly string _geocodeApiUrl = "https://geocoding-api.open-meteo.com/v1/search";
    private readonly string _airQualityApiUrl = "https://air-quality-api.open-meteo.com/v1/air-quality";
    private readonly HttpController httpController;

    /// <summary>
    /// Creates a new <seealso cref="OpenMeteoClient"/> object and sets the neccessary variables (httpController, CultureInfo)
    /// </summary>
    public OpenMeteoClient()
    {
        httpController = new HttpController();
    }

    /// <summary>
    /// Performs two GET-Requests (first geocoding api for latitude,longitude, then weather forecast)
    /// </summary>
    /// <param name="location">Name of city</param>
    /// <returns>If successful returns an awaitable Task containing WeatherForecast or NULL if request failed</returns>
    public async Task<WeatherForecast?> QueryAsync(string location)
    {
        GeocodingOptions geocodingOptions = new GeocodingOptions(location);

        // Get location Information
        GeocodingApiResponse? response = await GetGeocodingDataAsync(geocodingOptions);
        if (response == null || response.Locations == null)
            return null;

        WeatherForecastOptions options = new WeatherForecastOptions
        {
            Latitude = response.Locations[0].Latitude,
            Longitude = response.Locations[0].Longitude,
            Current = CurrentOptions.All // Get all current weather data if nothing else is provided

        };

        return await GetWeatherForecastAsync(options);
    }

    /// <summary>
    /// Performs two GET-Requests (first geocoding api for latitude,longitude, then weather forecast)
    /// </summary>
    /// <param name="options">Geocoding options</param>
    /// <returns>If successful awaitable <see cref="Task"/> or NULL</returns>
    public async Task<WeatherForecast?> QueryAsync(GeocodingOptions options)
    {
        // Get City Information
        GeocodingApiResponse? response = await GetLocationDataAsync(options);
        if (response == null || response?.Locations == null)
            return null;

        WeatherForecastOptions weatherForecastOptions = new WeatherForecastOptions
        {
            Latitude = response.Locations[0].Latitude,
            Longitude = response.Locations[0].Longitude,
            Current = CurrentOptions.All // Get all current weather data if nothing else is provided

        };

        return await GetWeatherForecastAsync(weatherForecastOptions);
    }

    /// <summary>
    /// Performs one GET-Request
    /// </summary>
    /// <param name="options"></param>
    /// <returns>Awaitable Task containing WeatherForecast or NULL</returns>
    public async Task<WeatherForecast?> QueryAsync(WeatherForecastOptions options)
    {
        try
        {
            return await GetWeatherForecastAsync(options);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Performs one GET-Request to get weather information
    /// </summary>
    /// <param name="latitude">City latitude</param>
    /// <param name="longitude">City longitude</param>
    /// <returns>Awaitable Task containing WeatherForecast or NULL</returns>
    public async Task<WeatherForecast?> QueryAsync(float latitude, float longitude)
    {
        WeatherForecastOptions options = new WeatherForecastOptions
        {
            Latitude = latitude,
            Longitude = longitude,

        };
        return await QueryAsync(options);
    }

    /// <summary>
    /// Gets Weather Forecast for a given location with individual options
    /// </summary>
    /// <param name="location"></param>
    /// <param name="options"></param>
    /// <returns><see cref="WeatherForecast"/> for the FIRST found result for <paramref name="location"/></returns>
    public async Task<WeatherForecast?> QueryAsync(string location, WeatherForecastOptions options)
    {
        GeocodingApiResponse? geocodingApiResponse = await GetLocationDataAsync(location);
        if (geocodingApiResponse == null || geocodingApiResponse?.Locations == null)
            return null;

        options.Longitude = geocodingApiResponse.Locations[0].Longitude;
        options.Latitude = geocodingApiResponse.Locations[0].Latitude;

        return await GetWeatherForecastAsync(options);
    }

    /// <summary>
    /// Gets air quality data for a given location with individual options
    /// </summary>
    /// <param name="options">options for air quality request</param>
    /// <returns><see cref="AirQuality"/> if successfull or <see cref="null"/> if failed</returns>
    public async Task<AirQuality?> QueryAsync(AirQualityOptions options)
    {
        return await GetAirQualityAsync(options);
    }

    /// <summary>
    /// Performs one GET-Request to Open-Meteo Geocoding API 
    /// </summary>
    /// <param name="location">Name of a location or city</param>
    /// <returns></returns>
    public async Task<GeocodingApiResponse?> GetLocationDataAsync(string location)
    {
        GeocodingOptions geocodingOptions = new GeocodingOptions(location);

        return await GetLocationDataAsync(geocodingOptions);
    }

    public async Task<GeocodingApiResponse?> GetLocationDataAsync(GeocodingOptions options)
    {
        return await GetGeocodingDataAsync(options);
    }

    /// <summary>
    /// Performs one GET-Request to get a (float, float) tuple
    /// </summary>
    /// <param name="location">Name of a city or location</param>
    /// <returns>(latitude, longitude) tuple of first found location or null if no location was found</returns>
    public async Task<(float latitude, float longitude)?> GetLocationLatitudeLongitudeAsync(string location)
    {
        GeocodingApiResponse? response = await GetLocationDataAsync(location);
        if (response == null || response?.Locations == null)
            return null;
        return (response.Locations[0].Latitude, response.Locations[0].Longitude);
    }

    public WeatherForecast? Query(WeatherForecastOptions options)
    {
        return QueryAsync(options).GetAwaiter().GetResult();
    }

    public WeatherForecast? Query(float latitude, float longitude)
    {
        return QueryAsync(latitude, longitude).GetAwaiter().GetResult();
    }

    public WeatherForecast? Query(string location, WeatherForecastOptions options)
    {
        return QueryAsync(location, options).GetAwaiter().GetResult();
    }

    public WeatherForecast? Query(GeocodingOptions options)
    {
        return QueryAsync(options).GetAwaiter().GetResult();
    }

    public WeatherForecast? Query(string location)
    {
        return QueryAsync(location).GetAwaiter().GetResult();
    }

    public AirQuality? Query(AirQualityOptions options)
    {
        return QueryAsync(options).GetAwaiter().GetResult();
    }

    private async Task<AirQuality?> GetAirQualityAsync(AirQualityOptions options)
    {
        try
        {
            HttpResponseMessage response = await httpController.Client.GetAsync(MergeUrlWithOptions(_airQualityApiUrl, options));
            response.EnsureSuccessStatusCode();

            AirQuality? airQuality = await JsonSerializer.DeserializeAsync<AirQuality>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return airQuality;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return null;
        }
    }

    private async Task<WeatherForecast?> GetWeatherForecastAsync(WeatherForecastOptions options)
    {
        try
        {
            HttpResponseMessage response = await httpController.Client.GetAsync(MergeUrlWithOptions(_weatherApiUrl, options));
            response.EnsureSuccessStatusCode();

            WeatherForecast? weatherForecast = await JsonSerializer.DeserializeAsync<WeatherForecast>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return weatherForecast;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return null;
        }

    }

    private async Task<GeocodingApiResponse?> GetGeocodingDataAsync(GeocodingOptions options)
    {
        try
        {
            HttpResponseMessage response = await httpController.Client.GetAsync(MergeUrlWithOptions(_geocodeApiUrl, options));
            response.EnsureSuccessStatusCode();

            GeocodingApiResponse? geocodingData = await JsonSerializer.DeserializeAsync<GeocodingApiResponse>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return geocodingData;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Can't find " + options.Name + ". Please make sure that the name is valid.");
            Console.WriteLine(e.Message);
            return null;
        }
    }

    private string MergeUrlWithOptions(string url, WeatherForecastOptions? options)
    {
        if (options == null) return url;

        UriBuilder uri = new UriBuilder(url);
        bool isFirstParam = false;

        // If no query given, add '?' to start the query string
        if (uri.Query == string.Empty)
        {
            uri.Query = "?";

            // isFirstParam becomes true because the query string is new
            isFirstParam = true;
        }

        // Add the properties

        // Begin with Latitude and Longitude since they're required
        if (isFirstParam)
            uri.Query += "latitude=" + options.Latitude.ToString(CultureInfo.InvariantCulture);
        else
            uri.Query += "&latitude=" + options.Latitude.ToString(CultureInfo.InvariantCulture);

        uri.Query += "&longitude=" + options.Longitude.ToString(CultureInfo.InvariantCulture);

        uri.Query += "&temperature_unit=" + options.Temperature_Unit.ToString();
        uri.Query += "&windspeed_unit=" + options.Windspeed_Unit.ToString();
        uri.Query += "&precipitation_unit=" + options.Precipitation_Unit.ToString();
        if (options.Timezone != string.Empty)
            uri.Query += "&timezone=" + options.Timezone;

        uri.Query += "&timeformat=" + options.Timeformat.ToString();

        uri.Query += "&past_days=" + options.Past_Days;

        if (options.Start_date != string.Empty)
            uri.Query += "&start_date=" + options.Start_date;
        if (options.End_date != string.Empty)
            uri.Query += "&end_date=" + options.End_date;

        // Now we iterate through hourly and daily

        // Hourly
        if (options.Hourly.Count > 0)
        {
            bool firstHourlyElement = true;
            uri.Query += "&hourly=";

            foreach (var option in options.Hourly)
            {
                if (firstHourlyElement)
                {
                    uri.Query += option.ToString();
                    firstHourlyElement = false;
                }
                else
                {
                    uri.Query += "," + option.ToString();
                }
            }
        }

        // Daily
        if (options.Daily.Count > 0)
        {
            bool firstDailyElement = true;
            uri.Query += "&daily=";
            foreach (var option in options.Daily)
            {
                if (firstDailyElement)
                {
                    uri.Query += option.ToString();
                    firstDailyElement = false;
                }
                else
                {
                    uri.Query += "," + option.ToString();
                }
            }
        }

        // 0.2.0 Weather models
        // cell_selection
        uri.Query += "&cell_selection=" + options.Cell_Selection;

        // Models
        if (options.Models.Count > 0)
        {
            bool firstModelsElement = true;
            uri.Query += "&models=";
            foreach (var option in options.Models)
            {
                if (firstModelsElement)
                {
                    uri.Query += option.ToString();
                    firstModelsElement = false;
                }
                else
                {
                    uri.Query += "," + option.ToString();
                }
            }
        }

        // new current parameter
        if (options.Current.Count > 0)
        {
            bool firstCurrentElement = true;
            uri.Query += "&current=";
            foreach (var option in options.Current)
            {
                if (firstCurrentElement)
                {
                    uri.Query += option.ToString();
                    firstCurrentElement = false;
                }
                else
                {
                    uri.Query += "," + option.ToString();
                }
            }
        }

        // new minutely_15 parameter
        if (options.Minutely15.Count > 0)
        {
            bool firstMinutelyElement = true;
            uri.Query += "&minutely_15=";
            foreach (var option in options.Minutely15)
            {
                if (firstMinutelyElement)
                {
                    uri.Query += option.ToString();
                    firstMinutelyElement = false;
                }
                else
                {
                    uri.Query += "," + option.ToString();
                }
            }
        }

        return uri.ToString();
    }

    /// <summary>
    /// Combines a given url with an options object to create a url for GET requests
    /// </summary>
    /// <returns>url+queryString</returns>
    private string MergeUrlWithOptions(string url, GeocodingOptions options)
    {
        if (options == null) return url;

        UriBuilder uri = new UriBuilder(url);
        bool isFirstParam = false;

        // If no query given, add '?' to start the query string
        if (uri.Query == string.Empty)
        {
            uri.Query = "?";

            // isFirstParam becomes true because the query string is new
            isFirstParam = true;
        }

        // Now we check every property and set the value, if neccessary
        if (isFirstParam)
            uri.Query += "name=" + options.Name;
        else
            uri.Query += "&name=" + options.Name;

        if (options.Count > 0)
            uri.Query += "&count=" + options.Count;

        if (options.Format != string.Empty)
            uri.Query += "&format=" + options.Format;

        if (options.Language != string.Empty)
            uri.Query += "&language=" + options.Language;

        return uri.ToString();
    }

    /// <summary>
    /// Combines a given url with an options object to create a url for GET requests
    /// </summary>
    /// <returns>url+queryString</returns>
    private string MergeUrlWithOptions(string url, AirQualityOptions options)
    {
        if (options == null) return url;

        UriBuilder uri = new UriBuilder(url);
        bool isFirstParam = false;

        // If no query given, add '?' to start the query string
        if (uri.Query == string.Empty)
        {
            uri.Query = "?";

            // isFirstParam becomes true because the query string is new
            isFirstParam = true;
        }

        // Now we check every property and set the value, if neccessary
        if (isFirstParam)
            uri.Query += "latitude=" + options.Latitude.ToString(CultureInfo.InvariantCulture);
        else
            uri.Query += "&latitude=" + options.Latitude.ToString(CultureInfo.InvariantCulture);

        uri.Query += "&longitude=" + options.Longitude.ToString(CultureInfo.InvariantCulture);

        if (options.Domains != string.Empty)
            uri.Query += "&domains=" + options.Domains;

        if (options.Timeformat != string.Empty)
            uri.Query += "&timeformat=" + options.Timeformat;

        if (options.Timezone != string.Empty)
            uri.Query += "&timezone=" + options.Timezone;

        // Finally add hourly array
        if (options.Hourly.Count >= 0)
        {
            bool firstHourlyElement = true;
            uri.Query += "&hourly=";

            foreach (var option in options.Hourly)
            {
                if (firstHourlyElement)
                {
                    uri.Query += option.ToString();
                    firstHourlyElement = false;
                }
                else
                {
                    uri.Query += "," + option.ToString();
                }
            }
        }

        return uri.ToString();
    }
}