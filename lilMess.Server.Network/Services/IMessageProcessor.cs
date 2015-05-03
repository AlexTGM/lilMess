namespace lilMess.Server.Network.Services
{
    using System;
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Packets.Body;
    using lilMess.Misc.Requests;

    public delegate void SendNewPacket(byte[] data, UserModel user = null, List<NetConnection> except = null);

    public interface IMessageProcessor
    {
        SendNewPacket SendNewPacket { get; set; }

        KeyValuePair<PacketType, Func<Request, string>> Chat { get; }

        KeyValuePair<PacketType, Func<Request, string>> Audio { get; }

        KeyValuePair<PacketType, Func<Request, string>> Connection { get; }

        string ConnectionApproval(NetIncomingMessage incomingMessage, AuthenticationBody body);

        string GetChatMessage(UserModel user, string message);

        string GetVoiceMessage(UserModel user, byte[] message);

        string MoveUser(UserModel user, RoomModel destinationRoom);
    }
}