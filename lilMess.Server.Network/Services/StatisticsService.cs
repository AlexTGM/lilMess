namespace lilMess.Server.Network.Services
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

            this.currentStatistics = this.previousStatistics = new StatisticsModel();

            this.timer = new Timer(o => this.GatherStatistics(), null, 0, 1000);
        }

        public GetStatistics Statistics { get; set; }

        private void GatherStatistics()
        {
            var recivedBytes = this.server.Statistics.ReceivedBytes;
            var sentBytes = this.server.Statistics.SentBytes;

            var temp = new StatisticsModel(recivedBytes, sentBytes);
            this.currentStatistics = temp - this.previousStatistics;

            var statistics = this.Statistics;
            if (statistics != null) statistics(this.currentStatistics);

            this.previousStatistics = temp;
        }
    }
}