namespace lilMess.Misc.Packets.Body
{
    using System;

    using lilMess.Misc.Model;

    [Serializable]
    public class ChatMessageBody : Body
    {
        public ChatMessageModel ChatMessageModel { get; set; } 
    }
}