namespace lilMess.Server.Network
{
    using lilMess.Server.Network.Models;
    using lilMess.Server.Network.Services;

    public delegate void GotMessage(string message);

    public delegate void Statistics(StatisticsModel stats);

    public interface INetwork
    {
        IService Service { get; }

        GotMessage GotMessage { get; set; }

        string StartupServer();

        void ShutdownServer();
    }
}