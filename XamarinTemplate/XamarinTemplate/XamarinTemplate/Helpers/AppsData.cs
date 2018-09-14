using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace XamarinTemplate.Helpers
{
    public static class AppsData
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string SampleText
        {
            get => AppSettings.GetValueOrDefault(nameof(SampleText), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(SampleText), value);

        }

        public static bool isLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(isLogin), false);

            set => AppSettings.AddOrUpdateValue(nameof(isLogin), value);

        }
    }
}
