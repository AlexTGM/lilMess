namespace lilMess.Server.Network.Services.Impl
{
    using System.Threading;

    using Lidgren.Network;

    using lilMess.Server.Network.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly NetServer server;

        private StatisticsModel previousStatistics;

        private StatisticsModel currentStatistics;

        private Timer timer;

        public StatisticsService(NetServer server)
        {
            this.server = server;

            currentStatistics = previousStatistics = new StatisticsModel();

            timer = new Timer(o => GatherStatistics(), null, 0, 1000);
        }

        public GetStatistics Statistics { get; set; }

        private void GatherStatistics()
        {
            var recivedBytes = server.Statistics.ReceivedBytes;
            var sentBytes = server.Statistics.SentBytes;

            var temp = new StatisticsModel(recivedBytes, sentBytes);
            currentStatistics = temp - previousStatistics;

            var statistics = Statistics;
            if (statistics != null) statistics(currentStatistics);

            previousStatistics = temp;
        }
    }
}