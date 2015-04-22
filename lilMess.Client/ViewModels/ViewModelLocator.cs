namespace lilMess.Client.ViewModels
{
    using lilMess.Client;
    using lilMess.Client.ViewModels.DesignTime;

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

        public LoginWindowViewModel Login
        {
            get { return Bootstrapper.Container.Resolve<LoginWindowViewModel>(); }
        }

        public RoomListViewModel RoomList
        {
            get { return Bootstrapper.Container.Resolve<RoomListViewModel>(); }
        }

        #region design time

        public RoomListDesignTimeViewModel RoomListDesignTime
        {
            get { return Bootstrapper.Container.Resolve<RoomListDesignTimeViewModel>(); }
        }

        #endregion
    }
}