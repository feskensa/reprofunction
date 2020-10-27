using Microsoft.Extensions.Configuration;

namespace Repro.Functions
{
    public static class ConfigurationExtension
    {
        public static string GetValueOrDefault(this IConfiguration configuration, string key)
        {
            var webAppSetting = configuration.GetWebAppSetting(key);

            return string.IsNullOrEmpty(webAppSetting) 
                ? configuration.GetLocalSetting(key) 
                : webAppSetting;
        }

        public static string GetWebAppSetting(this IConfiguration configuration, string key)
            => configuration.TryGetValue("APPSETTING_" + key);

        public static string GetLocalSetting(this IConfiguration configuration, string key)
            => configuration.TryGetValue(key);

        private static string TryGetValue(this IConfiguration configuration, string key)
        {
            return configuration.GetValue<string>(key) ?? string.Empty;
        }
    }
}