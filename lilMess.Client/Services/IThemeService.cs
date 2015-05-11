namespace lilMess.Client.Services
{
    using System.Collections.Generic;

    using MahApps.Metro;

    public interface IThemeService
    {
        IEnumerable<AppTheme> AvaiableAppThemes { get; }

        IEnumerable<Accent> AvaiableAccentsList { get; }

        void SetTheme(AppTheme theme);

        void SetAccent(Accent accent);

        AppTheme GetCurrentTheme();

        Accent GetCurrentAccent();
    }
}