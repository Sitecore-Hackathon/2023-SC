using SitecoreConfig = Sitecore.Configuration;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Helper
{
    public static class Settings
    {
        public static string GetSitecoreSettings(string key)
        {
            return SitecoreConfig.Settings.GetSetting(key);
        }
    }
}