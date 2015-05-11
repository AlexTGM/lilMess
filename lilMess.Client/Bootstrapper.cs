namespace lilMess.Client
{
    using System;

    using AutoMapper;

    using Lidgren.Network;

    using lilMess.Audio;
    using lilMess.Audio.Impl;
    using lilMess.Client.Network;
    using lilMess.Client.Network.Impl;
    using lilMess.Client.Services;
    using lilMess.Client.Services.Impl;
    using lilMess.Client.ViewModels;

    using Microsoft.Practices.Unity;

    public class Bootstrapper
    {
        public Bootstrapper()
        {
            Container = new UnityContainer();

            ConfigureContainer();
            ConfigureBindings();

            Container.Resolve<ApplicationCommon>().TranslationService.Language = Properties.Settings.Default.DefaultLanguage;
        }

        public IUnityContainer Container { get; private set; }

        private static void ConfigureBindings()
        {
            Mapper.CreateMap<Misc.Model.RoomModel, Models.RoomModel>();
            Mapper.CreateMap<Misc.Model.RoleModel, Models.RoleModel>();
            Mapper.CreateMap<Misc.Model.UserModel, Models.UserModel>();
            Mapper.CreateMap<Misc.Model.RoomModel, Models.RoomModel>();
            Mapper.CreateMap<Misc.Model.PermissionModel, Models.PermissionsModel>();

            Mapper.CreateMap<Models.RoomModel, Misc.Model.RoomModel>();
            Mapper.CreateMap<Models.UserModel, Misc.Model.UserModel>();
            Mapper.CreateMap<Models.RoleModel, Misc.Model.RoleModel>();
            Mapper.CreateMap<Models.PermissionsModel, Misc.Model.PermissionModel>();

            Mapper.CreateMap<Misc.Model.ChatMessageModel, Models.ChatMessageModel>()
                .ForMember(dest => dest.MessageTime, opt => opt.MapFrom(dest => DateTime.Now));
        }

        private void ConfigureContainer()
        {
            Container.RegisterType<INetwork, ClientNetwork>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAudioProcessor, AudioProcessor>(new ContainerControlledLifetimeManager());

            Container.RegisterType<MainViewModel>();
            Container.RegisterType<LoginViewModel>();
            Container.RegisterType<SettingsViewModel>();

            Container.RegisterType<ApplicationCommon>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ITranslationService, TranslationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IThemeService, ThemeService>(new ContainerControlledLifetimeManager());

            Container.RegisterInstance(typeof(NetClient), new NetClient(new NetPeerConfiguration("lilMess")), new ContainerControlledLifetimeManager());
        }
    }
}