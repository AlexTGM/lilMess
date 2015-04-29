namespace lilMess.Server.Network.Services
{
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc.Model;

    public interface IRoomService
    {
        List<RoomModel> RoomList { get; } 

        void AddUser(UserModel user, RoomModel room = null);

        UserModel FindUser(NetConnection connection);

        void RemoveUser(UserModel user);

        void MoveUserToRoom(UserModel user, RoomModel currentRoom, RoomModel destinationRoom);
    }
}