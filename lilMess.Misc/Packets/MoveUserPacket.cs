namespace lilMess.Misc.Packets
{
    using lilMess.Misc.Packets.Body;

    public class MoveUserPacket : Packet
    {
        public MoveUserPacket(MoveUserBody packetBody)
            : base((byte)Misc.PacketType.LogIn, packetBody) { }
    }
}