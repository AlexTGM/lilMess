namespace lilMess.Server.Network.Services
{
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc.Model;

    public interface IUserService
    {
        void AddUser(UserModel user, RoomModel room = null);

        UserModel FindUser(NetConnection connection);

        void RemoveUser(UserModel user);

        List<RoomModel> GetRoomsList();
    }
}