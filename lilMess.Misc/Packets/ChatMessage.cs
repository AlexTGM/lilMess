namespace lilMess.Misc.Packets
{
    using System;

    using lilMess.Misc;
    using lilMess.Misc.Packets.Body;

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public ChatMessagePacket(ChatMessageBody packetBody)
            : base((byte)lilMess.Misc.PacketType.ChatMessage, packetBody)
        {
        }
    }
}