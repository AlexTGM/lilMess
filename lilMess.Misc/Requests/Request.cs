namespace lilMess.Misc.Requests
{
    using Lidgren.Network;

    using lilMess.Misc.Model;
    using lilMess.Misc.Packets.Body;

    public class Request
    {
        public UserModel UserModel { get; set; }

        public NetIncomingMessage IncomingMessage { get; set; }

        public Body Body { get; set; }
    }
}