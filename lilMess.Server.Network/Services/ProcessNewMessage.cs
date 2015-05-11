namespace lilMess.Server.Network.Services
{
    using Lidgren.Network;

    using lilMess.Misc.Model;

    public delegate void ProcessNewMessage(NetIncomingMessage incomingMessage, UserModel user);
}