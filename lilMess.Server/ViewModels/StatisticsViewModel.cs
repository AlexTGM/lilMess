namespace lilMess.Server.ViewModels
{
    using GalaSoft.MvvmLight;

    using lilMess.Server.Models;
    using lilMess.Server.Network;

    public class StatisticsViewModel : ViewModelBase
    {
        private StatisticsModel statisticsModel = new StatisticsModel();

        public StatisticsViewModel(INetwork network)
        {
            network.Service.Statistics += delegate(Network.Models.StatisticsModel stats)
                {
                    this.statisticsModel.IncomingTraffic += stats.IncomingTraffic;
                    this.statisticsModel.OutcomingTraffic += stats.OutcomingTraffic;
                };
        }

        public StatisticsModel StatisticsModel
        {
            get { return this.statisticsModel; }
            set { this.Set("StatisticsModel", ref this.statisticsModel, value); }
        }
    }
}