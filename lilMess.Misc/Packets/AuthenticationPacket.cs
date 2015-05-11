namespace lilMess.Misc.Packets
{
    using System;

    using lilMess.Misc;
    using lilMess.Misc.Packets.Body;

    [Serializable]
    public class AuthenticationPacket : Packet
    {
        public AuthenticationPacket(AuthenticationBody packetBody)
            : base((byte)Misc.PacketType.LogIn, packetBody) { }
    }
}