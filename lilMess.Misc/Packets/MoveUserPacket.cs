namespace lilMess.Misc.Packets
{
    using System;

    using lilMess.Misc.Packets.Body;

    [Serializable]
    public class MoveUserPacket : Packet
    {
        public MoveUserPacket(MoveUserBody packetBody)
            : base((byte)Misc.PacketType.MoveUser, packetBody)
        {
        }
    }
}