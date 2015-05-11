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
            if (Bootstrapper == null)
            {
                Bootstrapper = new Bootstrapper();
            }
        }

        public MainViewModel Main
        {
            get { return Bootstrapper.Container.Resolve<MainViewModel>(); }
        }

        public LoginViewModel Login
        {
            get { return Bootstrapper.Container.Resolve<LoginViewModel>(); }
        }

        public RoomListViewModel RoomList
        {
            get { return Bootstrapper.Container.Resolve<RoomListViewModel>(); }
        }

        public SettingsViewModel Settings
        {
            get { return Bootstrapper.Container.Resolve<SettingsViewModel>(); }
        }

        #region design time

        public RoomListDesignTimeViewModel RoomListDesignTime
        {
            get { return Bootstrapper.Container.Resolve<RoomListDesignTimeViewModel>(); }
        }

        #endregion
    }
}