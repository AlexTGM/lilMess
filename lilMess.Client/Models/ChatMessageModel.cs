namespace lilMess.Client.Models
{
    using System;

    using GalaSoft.MvvmLight;

    public class ChatMessageModel : ObservableObject
    {
        private UserModel messageSender;

        private string messageContent;

        private DateTime messageTime;

        public DateTime MessageTime
        {
            get { return messageTime; }
            set { Set("MessageTime", ref messageTime, value); }
        }

        public string MessageContent
        {
            get { return messageContent; }
            set { Set("MessageContent", ref messageContent, value); }
        }

        public UserModel MessageSender
        {
            get { return messageSender; }
            set { Set("MessageSender", ref messageSender, value); }
        }
    }
}