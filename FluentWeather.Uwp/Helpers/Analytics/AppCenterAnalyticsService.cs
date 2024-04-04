using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Uwp.Shared;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using AppCenter = Microsoft.AppCenter.Analytics.Analytics;
using AppCenterService = Microsoft.AppCenter.AppCenter;


namespace FluentWeather.Uwp.Helpers.Analytics;

public class AppCenterAnalyticsService : AppAnalyticsService
{
    public override void Start()
    {
        base.Start();
        AppCenterService.Start("507a5f67-6c14-432d-bcc3-4619144ecd38", typeof(AppCenter), typeof(Crashes));
    }
    public override void TrackEvent(string name, IDictionary<string, string> properties = null, bool addDefaultProperties = true)
    {
        if (!IsStarted) return;
        base.TrackEvent(name, properties, addDefaultProperties);
        AppCenter.TrackEvent(name, properties);
    }
}