namespace lilMess.Server.Network.Services
{
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc.Model;

    public interface IRoomService
    {
        List<RoomModel> RoomList { get; } 

        void AddUser(UserModel user, RoomModel room = null);

        UserModel FindUser(string id);
        UserModel FindUser(NetConnection connection);

        void RemoveUser(string id);

        void MoveUserToRoom(string id, string roomName);

        RoomModel GetUserCurrentRoom(string id);
    }
}