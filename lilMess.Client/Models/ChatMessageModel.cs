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
            get { return this.messageTime; }
            set { this.Set("MessageTime", ref this.messageTime, value); }
        }

        public string MessageContent
        {
            get { return this.messageContent; }
            set { this.Set("MessageContent", ref this.messageContent, value); }
        }

        public UserModel MessageSender
        {
            get { return this.messageSender; }
            set { this.Set("MessageSender", ref this.messageSender, value); }
        }
    }
}