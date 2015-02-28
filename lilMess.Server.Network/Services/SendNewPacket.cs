namespace lilMess.Server.Network.Services
{
    using System.Collections.Generic;

    using Lidgren.Network;

    public delegate void SendNewPacket(byte[] data, List<NetConnection> exceptRecipients);
}