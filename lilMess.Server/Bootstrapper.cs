namespace lilMess.Server
{
    using AutoMapper;

    using Lidgren.Network;

    using lilMess.DataAccess;
    using lilMess.DataAccess.Impl;
    using lilMess.DataAccess.Models;
    using lilMess.Misc.Model;
    using lilMess.Server.Network;
    using lilMess.Server.Network.Impl;
    using lilMess.Server.Network.Services;
    using lilMess.Server.Network.Services.Impl;
    using lilMess.Server.ViewModels;

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
            Container.RegisterType<IRepositoryManager, RepositoryManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRoomService, RoomService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IService, Service>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IStatisticsService, StatisticsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<INetwork, ServerNetwork>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMessageProcessor, MessageProcessor>(new ContainerControlledLifetimeManager());

            Container.RegisterType<MainWindowViewModel>();
            Container.RegisterType<StatisticsVeiewModel>();

            var config = new NetPeerConfiguration("lilMess") { MaximumConnections = 100, Port = 9997 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            Container.RegisterInstance(typeof(NetServer), new NetServer(config), new ContainerControlledLifetimeManager());
        }

        private void ConfigureBindings()
        {
            Mapper.CreateMap<User, UserModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid));

            Mapper.CreateMap<UserModel, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid));

            Mapper.CreateMap<Room, RoomModel>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RoomParent, opt => opt.MapFrom(src => src.ParentRoom))
                .ForMember(dest => dest.RoomIsHome, opt => opt.MapFrom(src => src.Home));

            Mapper.CreateMap<RoomModel, Room>();

            Mapper.CreateMap<Role, RoleModel>()
                .ForMember(dest => dest.RoleColor, opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));

            Mapper.CreateMap<Permisson, PermissionModel>();

            Mapper.CreateMap<PermissionModel, Permisson>();

            Mapper.CreateMap<Network.Models.StatisticsModel, Models.StatisticsModel>();
        }
    }
}