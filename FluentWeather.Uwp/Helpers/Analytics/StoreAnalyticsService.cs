using Microsoft.AppCenter;
using Microsoft.Services.Store.Engagement;
namespace FluentWeather.Uwp.Helpers.Analytics;

public class StoreAnalyticsService : AppAnalyticsService
{
    public override void TrackEvent(string name, IDictionary<string, string> properties = null, bool addDefaultProperties = true)
    {
        StoreServicesCustomEventLogger logger = StoreServicesCustomEventLogger.GetDefault();
        logger.Log(name);
    }
}