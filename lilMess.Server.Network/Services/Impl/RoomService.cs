﻿namespace lilMess.Server.Network.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Lidgren.Network;

    using lilMess.DataAccess;
    using lilMess.Misc.Model;

    public class RoomService : IRoomService
    {
        private readonly List<RoomModel> rooms = new List<RoomModel>();

        public RoomService(IRepositoryManager manager)
        {
            var roomList = manager.RoomRepository.Select(x => x);

            this.rooms.AddRange(Mapper.Map<IEnumerable<RoomModel>>(roomList));
        }

        public List<RoomModel> RoomList { get { return this.rooms; } }

        public void AddUser(UserModel user, RoomModel room = null)
        {
            (room ?? this.rooms.First(x => x.RoomIsHome)).RoomUsers.Add(user);
        }

        public UserModel FindUser(NetConnection connection)
        {
            return this.rooms.Select(room => room.RoomUsers.FirstOrDefault(x => x.Connection == connection)).FirstOrDefault();
        }

        public void RemoveUser(UserModel user)
        {
            foreach (var room in this.rooms.Where(room => room.RoomUsers.Contains(user))) room.RoomUsers.Remove(user);
        }

        public void MoveUserToRoom(UserModel user, RoomModel currentRoom, RoomModel destinationRoom)
        {
            currentRoom.RoomUsers.Remove(user);
            destinationRoom.RoomUsers.Add(user);
        }

        public RoomModel GetUserCurrentRoom(UserModel user)
        {
            return this.RoomList.SingleOrDefault(room => room.RoomUsers.Contains(user));
        }
    }
}