namespace lilMess.Misc
{
    using System;

    using lilMess.Misc.Packets.Body;

    public enum PacketType : byte
    {
        ChatMessage = 1,
        VoiceMessage = 2,
        LogIn = 3,
        ServerMessage = 5,
    }

    [Serializable]
    public abstract class Packet
    {
        private readonly byte packetType;

        private readonly Body packetBody;

        protected Packet() { }

        protected Packet(byte packetType, Body packetBody)
        {
            this.packetType = packetType;
            this.packetBody = packetBody;
        }

        public byte PacketType { get { return this.packetType; } }

        public Body PacketBody { get { return this.packetBody; } }
    }
}