namespace lilMess.Client.Views
{
    using lilMess.Client.ViewModels;

    using MahApps.Metro.Controls;

    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var viewModel = (MainWindowViewModel)this.DataContext;

            this.KeyDown += viewModel.OnKeyDown;
            this.KeyUp += viewModel.OnKeyUp;
            this.Closed += viewModel.OnWindowClosing;
        }
    }
}