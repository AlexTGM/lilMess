﻿namespace lilMess.Server.Network.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Lidgren.Network;

    using lilMess.DataAccess;
    using lilMess.DataAccess.Models;
    using lilMess.Misc.Model;

    public class UserService : IUserService
    {
        private readonly IRepositoryManager repositoryManager;

        private readonly List<RoomModel> rooms = new List<RoomModel>();

        public UserService(IRepositoryManager manager)
        {
            this.repositoryManager = manager;

            var roomList = this.repositoryManager.RoomRepository.Select(x => x);

            this.rooms.AddRange(Mapper.Map<IEnumerable<Room>, IEnumerable<RoomModel>>(roomList));
        }

        public void AddUser(UserModel user, RoomModel room = null)
        {
            (room ?? this.rooms.First(x => x.Home)).Users.Add(user);
        }

        public UserModel FindUser(NetConnection connection)
        {
            return this.rooms.Select(room => room.Users.FirstOrDefault(x => x.Connection == connection)).FirstOrDefault();
        }

        public void RemoveUser(UserModel user)
        {
            foreach (var room in this.rooms.Where(room => room.Users.Contains(user))) room.Users.Remove(user);
        }

        public List<RoomModel> GetRoomsList()
        {
            return this.rooms.ToList();
        }
    }
}