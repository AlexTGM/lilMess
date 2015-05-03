namespace lilMess.Server.Network.Services.Impl
{
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

        public void RemoveUser(UserModel user)
        {
            foreach (var room in _rooms.Where(room => room.RoomUsers.Contains(user))) room.RoomUsers.Remove(user);
        }

        public void MoveUserToRoom(UserModel user, RoomModel destinationRoom)
        {
            var currentRoom = RoomList.Single(room => room.RoomUsers.Contains(user));

            currentRoom.RoomUsers.Remove(user);
            destinationRoom.RoomUsers.Add(user);
        }

        public RoomModel GetUserCurrentRoom(UserModel user)
        {
            return RoomList.SingleOrDefault(room => room.RoomUsers.Contains(user));
        }
    }
}