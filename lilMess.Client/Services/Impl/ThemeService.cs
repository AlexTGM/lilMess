namespace lilMess.Client.Services.Impl
{
    using System.Collections.Generic;
    using System.Windows;

    using MahApps.Metro;

    public class ThemeService : IThemeService
    {
        public ThemeService()
        {
            var appTheme = Properties.Settings.Default.DefaultAppTheme;
            var appAccent = Properties.Settings.Default.DefaultAppAccent;

            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(appAccent), ThemeManager.GetAppTheme(appTheme));
        }

        public IEnumerable<AppTheme> AvaiableAppThemes
        {
            get
            {
                return ThemeManager.AppThemes;
            }
        }

        public IEnumerable<Accent> AvaiableAccentsList
        {
            get
            {
                return ThemeManager.Accents;
            }
        }

        public void SetTheme(AppTheme theme)
        {
            Properties.Settings.Default.DefaultAppTheme = theme.Name;

            ChangeAppStyle(theme, null);
        }

        public void SetAccent(Accent accent)
        {
            Properties.Settings.Default.DefaultAppAccent = accent.Name;

            ChangeAppStyle(null, accent);
        }

        public AppTheme GetCurrentTheme()
        {
            return ThemeManager.DetectAppStyle(Application.Current).Item1;
        }

        public Accent GetCurrentAccent()
        {
            return ThemeManager.DetectAppStyle(Application.Current).Item2;
        }

        private void ChangeAppStyle(AppTheme theme, Accent accent)
        {
            ThemeManager.ChangeAppStyle(Application.Current, accent ?? GetCurrentAccent(), theme ?? GetCurrentTheme());
        }
    }
}