namespace lilMess.Server.Network.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Lidgren.Network;

    using lilMess.DataAccess;
    using lilMess.Misc.Model;

    public class RoomService : IRoomService
    {
        private readonly List<RoomModel> _rooms = new List<RoomModel>();

        public RoomService(IRepositoryManager manager)
        {
            var roomList = manager.RoomRepository.Select(x => x);

            _rooms.AddRange(Mapper.Map<IEnumerable<RoomModel>>(roomList));
        }

        public List<RoomModel> RoomList { get { return _rooms; } }

        public void AddUser(UserModel user, RoomModel room = null)
        {
            (room ?? _rooms.First(x => x.RoomIsHome)).RoomUsers.Add(user);
        }

        public UserModel FindUser(NetConnection connection)
        {
            return _rooms.Select(room => room.RoomUsers.FirstOrDefault(x => x.Connection == connection)).FirstOrDefault();
        }

        public UserModel FindUser(string id)
        {
            return _rooms.Select(room => room.RoomUsers.FirstOrDefault(x => x.Id == id)).FirstOrDefault(user => user != null);
        }

        public void RemoveUser(string id)
        {
            foreach (var room in _rooms.Where(room => room.RoomUsers.Any(user => user.Id == id)))
            {
                var userToDelete = room.RoomUsers.SingleOrDefault(user => user.Id == id);
        
                if (userToDelete != null) room.RoomUsers.Remove(userToDelete);
            }
        }

        public void MoveUserToRoom(string id, string roomName)
        {
            var userToDelete = FindUser(id);

            if (userToDelete == null) throw new ArgumentException(userToDelete.ToString());

            var currentRoom = RoomList.Single(room => room.RoomUsers.Any(user => user.Id == id));
            currentRoom.RoomUsers.Remove(userToDelete);
            RoomList.Single(room => Equals(room.RoomName, roomName)).RoomUsers.Add(userToDelete);

            //var currentRoom = RoomList.Single(room => room.RoomUsers.Contains(user => user.));

            //currentRoom.RoomUsers.Remove(user);

            //_rooms.Single(room => Equals(room.RoomName, destinationRoom.RoomName)).RoomUsers.Add(user);
        }

        public RoomModel GetUserCurrentRoom(string id)
        {
            return RoomList.SingleOrDefault(room => room.RoomUsers.Any(user => user.Id == id));
        }
    }
}