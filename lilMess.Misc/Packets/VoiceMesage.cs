namespace lilMess.Misc.Packets
{
    using System;

    using lilMess.Misc;
    using lilMess.Misc.Packets.Body;

    [Serializable]
    public class VoiceMessagePacket : Packet
    {
        public VoiceMessagePacket(VoiceMessageBody packetBody)
            : base((byte)Misc.PacketType.VoiceMessage, packetBody) { }
    }
}