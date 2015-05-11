namespace lilMess.Misc
{
    using System;

    using lilMess.Misc.Packets.Body;

    public enum PacketType : byte
    {
        ChatMessage = 1,
        VoiceMessage = 2,
        LogIn = 3,
        MoveUser = 4,
        ServerMessage = 5
    }

    [Serializable]
    public abstract class Packet
    {
        private readonly byte _packetType;

        private readonly Body _packetBody;

        protected Packet() { }

        protected Packet(byte packetType, Body packetBody)
        {
            _packetType = packetType;
            _packetBody = packetBody;
        }

        public byte PacketType { get { return _packetType; } }

        public Body PacketBody { get { return _packetBody; } }
    }
}