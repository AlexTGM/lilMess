namespace lilMess.Client.Views
{
    using System.Windows;

    using lilMess.Client.ViewModels;

    using MahApps.Metro;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var theme = ThemeManager.GetAppTheme("BaseLight");
            var accent = ThemeManager.GetAccent("Pink");
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme);

            var viewModel = (MainWindowViewModel)DataContext;
            Closed += viewModel.OnWindowClosing;
        }
    }
}