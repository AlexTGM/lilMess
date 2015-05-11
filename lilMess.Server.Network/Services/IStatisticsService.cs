namespace lilMess.Server.Network.Services
{
    using lilMess.Server.Network.Models;

    public delegate void GetStatistics(StatisticsModel statistics);

    public interface IStatisticsService
    {
        GetStatistics Statistics { get; set; }
    }
}