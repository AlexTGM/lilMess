namespace lilMess.Misc.Packets.Body
{
    using System;

    [Serializable]
    public class ChatMessageBody : Body
    {
        public string Sender { get; set; }

        public string Message { get; set; }
    }
}