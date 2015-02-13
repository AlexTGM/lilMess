namespace lilMess.Server.Network
{
    using lilMess.Server.Network.Models;

    public delegate void GotMessage(string message);

    public delegate void Statistics(StatisticsModel stats);

    public interface INetwork
    {
        GotMessage GotMessage { get; set; }

        Statistics Statistics { get; set; }

        string StartupServer();

        void ShutdownServer();
    }
}