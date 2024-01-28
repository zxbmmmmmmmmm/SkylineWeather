using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentWeather.OpenMeteoApi.Models;

public class Minutely15Options : IEnumerable, ICollection<Minutely15OptionsParameter>
{
    /// <summary>
    /// Gets a new object containing every parameter
    /// </summary>
    /// <returns></returns>
    public static Minutely15Options All => new((Minutely15OptionsParameter[])Enum.GetValues(typeof(Minutely15OptionsParameter)));

    /// <summary>
    /// Gets a copy of elements contained in the List.
    /// </summary>
    /// <typeparam name="Minutely15OptionsParameter"></typeparam>
    /// <returns>A copy of elements contained in the List</returns>
    public List<Minutely15OptionsParameter> Parameter => new(_parameter);

    public int Count => _parameter.Count;

    public bool IsReadOnly => false;

    private readonly List<Minutely15OptionsParameter> _parameter = new List<Minutely15OptionsParameter>();

    public Minutely15Options()
    {

    }

    public Minutely15Options(Minutely15OptionsParameter parameter)
    {
        Add(parameter);
    }

    public Minutely15Options(Minutely15OptionsParameter[] parameter)
    {
        Add(parameter);
    }

    /// <summary>
    /// Index the collection
    /// </summary>
    /// <param name="index"></param>
    /// <returns><see cref="string"/> Minutely15OptionsParameter as string representation at index</returns>
    public Minutely15OptionsParameter this[int index]
    {
        get { return _parameter[index]; }
        set
        {
            _parameter[index] = value;
        }
    }

    public void Add(Minutely15OptionsParameter param)
    {
        // Check that the parameter isn't already added
        if (_parameter.Contains(param)) return;

        _parameter.Add(param);
    }

    public void Add(Minutely15OptionsParameter[] param)
    {
        foreach (Minutely15OptionsParameter paramToAdd in param)
        {
            Add(paramToAdd);
        }
    }

    public void Clear()
    {
        _parameter.Clear();
    }

    public bool Contains(Minutely15OptionsParameter item)
    {
        return _parameter.Contains(item);
    }

    public bool Remove(Minutely15OptionsParameter item)
    {
        return _parameter.Remove(item);
    }

    public void CopyTo(Minutely15OptionsParameter[] array, int arrayIndex)
    {
        _parameter.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Minutely15OptionsParameter> GetEnumerator()
    {
        return _parameter.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

public enum Minutely15OptionsParameter
{
    temperature_2m, 
    relativehumidity_2m, 
    dewpoint_2m, 
    apparent_temperature, 
    precipitation, 
    rain, 
    snowfall, 
    snowfall_height, 
    freezinglevel_height, 
    weathercode, 
    windspeed_10m, 
    windspeed_80m, 
    winddirection_10m, 
    winddirection_80m, 
    windgusts_10m, 
    visibility, 
    cape, 
    lightning_potential, 
    shortwave_radiation, 
    direct_radiation, 
    diffuse_radiation, 
    direct_normal_irradiance, 
    terrestrial_radiation, 
    shortwave_radiation_instant, 
    direct_radiation_instant, 
    diffuse_radiation_instant, 
    direct_normal_irradiance_instant, 
    terrestrial_radiation_instant
}