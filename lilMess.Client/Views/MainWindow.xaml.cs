namespace lilMess.Client.Views
{
    using System.Windows;

    using lilMess.Client.ViewModels;

    using MahApps.Metro;
    using MahApps.Metro.Controls;

    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            //var theme = ThemeManager.GetAppTheme("BaseLight");
            //var accent = ThemeManager.GetAccent("Pink");
            //ThemeManager.ChangeAppStyle(Application.Current, accent, theme);

            var viewModel = (MainWindowViewModel)this.DataContext;

            this.KeyDown += viewModel.OnKeyDown;
            this.KeyUp += viewModel.OnKeyUp;
            this.Closed += viewModel.OnWindowClosing;
        }
    }
}