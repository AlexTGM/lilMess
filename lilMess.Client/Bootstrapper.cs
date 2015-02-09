namespace lilMess.Client
{
    using lilMess.Audio;
    using lilMess.Audio.Impl;
    using lilMess.Client.Network;
    using lilMess.Client.Network.Impl;
    using lilMess.Client.ViewModels;

    using Microsoft.Practices.Unity;

    public class Bootstrapper
    {
        public Bootstrapper()
        {
            this.Container = new UnityContainer();
            this.ConfigureContainer();
        }

        public IUnityContainer Container { get; private set; }
        
        private void ConfigureContainer()
        {
            this.Container.RegisterInstance<INetwork>(new ClientNetwork(), new ContainerControlledLifetimeManager());
            this.Container.RegisterInstance<IAudioProcessor>(new AudioProcessor(), new ContainerControlledLifetimeManager());

            this.Container.RegisterType<MainWindowViewModel>();
            this.Container.RegisterType<LoginWindowViewModel>();
        }
    }
}