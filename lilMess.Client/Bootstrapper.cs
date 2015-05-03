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
            Container = new UnityContainer();
            ConfigureContainer();
            ConfigureBindings();
        }

        public IUnityContainer Container { get; private set; }

        private void ConfigureContainer()
        {
            Container.RegisterType<INetwork, ClientNetwork>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAudioProcessor, AudioProcessor>(new ContainerControlledLifetimeManager());

            Container.RegisterType<MainWindowViewModel>();
            Container.RegisterType<LoginWindowViewModel>();

            Container.RegisterInstance(typeof(NetClient), new NetClient(new NetPeerConfiguration("lilMess")), new ContainerControlledLifetimeManager());
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