namespace lilMess.Client.ViewModels
{
    using lilMess.Client;

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
            get
            {
                return Bootstrapper.Container.Resolve<MainWindowViewModel>();
            }
        }

        public LoginWindowViewModel Login
        {
            get
            {
                return Bootstrapper.Container.Resolve<LoginWindowViewModel>();
            }
        }
    }
}