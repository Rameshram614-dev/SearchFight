using System.Configuration;

namespace SearchFight.Helper.Config
{
    public class ConfigManager
    {
        public static string GoogleUri => GetSettingByKey("GoogleURL");
        public static string BingUri => GetSettingByKey("BingURL");
        public static string GoogleKey => GetSettingByKey("GoogleAPIKey");
        public static string GoogleCxKey => GetSettingByKey("GoogleCxKey");
        public static string BingKey => GetSettingByKey("BingKey");

        public static string GetSettingByKey(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return value;
        }
    }
}
