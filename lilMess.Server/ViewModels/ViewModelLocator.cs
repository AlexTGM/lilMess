namespace lilMess.Server.ViewModels
{
    using lilMess.Server;

    using Microsoft.Practices.Unity;

    public class ViewModelLocator
    {
        private static readonly Bootstrapper Bootstrapper;

        static ViewModelLocator()
        {
            if (Bootstrapper == null) Bootstrapper = new Bootstrapper();
        }

        public MainWindowViewModel Main
        {
            get { return Bootstrapper.Container.Resolve<MainWindowViewModel>(); }
        }

        public StatisticsViewModel Statistics
        {
            get { return Bootstrapper.Container.Resolve<StatisticsViewModel>(); }
        }
    }
}