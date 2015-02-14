namespace lilMess.Server.Network.Services
{
    using System;
    using System.Collections.Generic;

    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Requests;

    public delegate void SendNewPacket(byte[] data, List<NetConnection> exceptRecipients);

    public interface IMessageProcessor
    {
        SendNewPacket SendNewPacket { get; set; }

        KeyValuePair<PacketType, Func<Request, string>> Chat { get; set; }

        KeyValuePair<PacketType, Func<Request, string>> Audio { get; set; }

        KeyValuePair<PacketType, Func<Request, string>> Connection { get; set; }

        string ConnectionApproval(NetIncomingMessage incomingMessage, string guid, string login);

        string GetChatMessage(UserModel user, string message);

        string GetVoiceMessage(UserModel user, byte[] message);
    }
}