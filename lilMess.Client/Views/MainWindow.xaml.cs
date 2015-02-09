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

            this.KeyDown += (sender, args) => viewModel.OnKeyDown(args);
            this.KeyUp += (sender, args) => viewModel.OnKeyUp(args);
        }
    }
}