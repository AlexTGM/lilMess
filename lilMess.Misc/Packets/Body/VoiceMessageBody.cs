namespace lilMess.Misc.Packets.Body
{
    using System;

    [Serializable]
    public class VoiceMessageBody : Body
    {
        public byte[] Message { get; set; }
    }
}