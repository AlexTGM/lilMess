namespace lilMess.Client.ViewModels
{
    using System.Globalization;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    
    using lilMess.Client.Views;

    using MahApps.Metro;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly ApplicationCommon _applicationCommon;

        public SettingsViewModel(ApplicationCommon applicationCommon)
        {
            _applicationCommon = applicationCommon;

            SaveSettingsCommand = new RelayCommand<object>(SaveSettings);
            CancelCommand = new RelayCommand<object>(CloseWindow);

            Language = _applicationCommon.TranslationService.Language;
            Theme = _applicationCommon.ThemeService.GetCurrentTheme();
            Accent = _applicationCommon.ThemeService.GetCurrentAccent();
        }

        public RelayCommand<object> SaveSettingsCommand { get; private set; }

        public RelayCommand<object> CancelCommand { get; private set; }

        public CultureInfo Language { get; set; }

        public AppTheme Theme { get; set; }

        public Accent Accent { get; set; }

        public ApplicationCommon ApplicationCommon
        {
            get
            {
                return _applicationCommon;
            }
        }

        private void SaveSettings(object param)
        {
            _applicationCommon.TranslationService.Language = Language;
            _applicationCommon.ThemeService.SetAccent(Accent);
            _applicationCommon.ThemeService.SetTheme(Theme);

            CloseWindow(param);
        }

        private void CloseWindow(object param)
        {
            ((SettingsView)param).Hide();
        }
    }
}