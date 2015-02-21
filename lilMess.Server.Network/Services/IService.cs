namespace lilMess.Server.Network.Services
{
    using Lidgren.Network;

    using lilMess.Misc;
    using lilMess.Misc.Model;
    using lilMess.Misc.Requests;
    using lilMess.Server.Network.Models;

    public delegate void ProcessNewMessage(NetIncomingMessage incomingMessage, UserModel user);

    public interface IService
    {
        StatisticsModel Stats { get; }

        void StartupServer(ProcessNewMessage processNewMessageDelegate);

        void ShutdownServer();

        string InvokeMethod(Packet packet, Request request);

        string StatusChanged(UserModel user, NetIncomingMessage incomingMessage);
    }
}