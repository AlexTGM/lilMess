namespace lilMess.Server.Network.Services.Impl
{
    using System.Threading;

    using Lidgren.Network;

    using lilMess.Server.Network.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly NetServer _server;

        private StatisticsModel _previousStatistics;

        private StatisticsModel _currentStatistics;

        private Timer _timer;

        public StatisticsService(NetServer server)
        {
            _server = server;

            _currentStatistics = _previousStatistics = new StatisticsModel();

            _timer = new Timer(o => GatherStatistics(), null, 0, 1000);
        }

        public GetStatistics Statistics { get; set; }

        private void GatherStatistics()
        {
            var recivedBytes = _server.Statistics.ReceivedBytes;
            var sentBytes = _server.Statistics.SentBytes;

            var temp = new StatisticsModel(recivedBytes, sentBytes);
            _currentStatistics = temp - _previousStatistics;

            var statistics = Statistics;
            if (statistics != null)
            {
                statistics(_currentStatistics);
            }

            _previousStatistics = temp;
        }
    }
}