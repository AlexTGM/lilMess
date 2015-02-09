namespace lilMess.Misc.Packets
{
    using System;

    using lilMess.Misc;
    using lilMess.Misc.Packets.Body;

    [Serializable]
    public class ServerInfoPacket : Packet
    {
        public ServerInfoPacket(ServerInfoBody packetBody)
            : base((byte)Misc.PacketType.ServerMessage, packetBody)
        {
        }
    }
}