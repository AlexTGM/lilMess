namespace lilMess.Client.Views
{
    using lilMess.Client.ViewModels;

    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();

            var viewModel = (MainViewModel)DataContext;
            Closed += viewModel.OnWindowClosing;
        }
    }
}