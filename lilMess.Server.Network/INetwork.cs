namespace lilMess.Server.Network
{
    using lilMess.Server.Network.Services;

    public delegate void GotMessage(string message);

    public interface INetwork
    {
        GotMessage GotMessage { get; set; }

        IStatisticsService StatisticsService { get; }

        string StartupServer();

        void ShutdownServer();
    }
}