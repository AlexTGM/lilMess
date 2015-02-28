namespace lilMess.Misc.Model
{
    using System;

    [Serializable]
    public class ChatMessageModel
    {
        public string MessageContent { get; set; }

        public UserModel MessageSender { get; set; }
    }
}