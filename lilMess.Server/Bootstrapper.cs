﻿namespace lilMess.Server
{
    using AutoMapper;

    using lilMess.DataAccess;
    using lilMess.DataAccess.Impl;
    using lilMess.DataAccess.Models;
    using lilMess.Misc.Model;
    using lilMess.Server.Network;
    using lilMess.Server.Network.Impl;
    using lilMess.Server.Network.Services;
    using lilMess.Server.ViewModels;

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
            this.Container.RegisterType<IRepositoryManager, RepositoryManager>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IService, Service>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<INetwork, ServerNetwork>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<MainWindowViewModel>();
        }

        private void ConfigureBindings()
        {
            Mapper.CreateMap<User, UserModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid));

            Mapper.CreateMap<UserModel, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid));

            Mapper.CreateMap<Room, RoomModel>();

            Mapper.CreateMap<RoomModel, Room>();
        }
    }
}