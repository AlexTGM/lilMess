namespace lilMess.Server.Models
{
    public class MessageModel
    {
        public string Message { get; set; }

        public override string ToString()
        {
            return this.Message;
        }
    }
}