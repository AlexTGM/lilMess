namespace lilMess.Server.Network
{
    public delegate void GotMessage(string message);

    public interface INetwork
    {
        string StartupServer(GotMessage gotMessageDelegate);

        void ShutdownServer();
    }
}