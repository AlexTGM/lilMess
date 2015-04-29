namespace lilMess.Client
{
    using System;

    using AutoMapper;

    using Lidgren.Network;

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
            this.ConfigureBindings();
        }

        public IUnityContainer Container { get; private set; }

        private void ConfigureContainer()
        {
            this.Container.RegisterType<INetwork, ClientNetwork>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IAudioProcessor, AudioProcessor>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<MainWindowViewModel>();
            this.Container.RegisterType<LoginWindowViewModel>();

            this.Container.RegisterInstance(typeof(NetClient), new NetClient(new NetPeerConfiguration("lilMess")), new ContainerControlledLifetimeManager());
        }

        private void ConfigureBindings()
        {
            Mapper.CreateMap<Misc.Model.RoomModel, Models.RoomModel>();
            Mapper.CreateMap<Misc.Model.RoleModel, Models.RoleModel>();
            Mapper.CreateMap<Misc.Model.UserModel, Models.UserModel>();
            Mapper.CreateMap<Misc.Model.PermissionModel, Models.PermissionsModel>();

            Mapper.CreateMap<Misc.Model.ChatMessageModel, Models.ChatMessageModel>()
                .ForMember(dest => dest.MessageTime, opt => opt.MapFrom(dest => DateTime.Now));
        }
    }
}