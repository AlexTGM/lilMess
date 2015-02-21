namespace lilMess.Server.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Threading;

    using AutoMapper;

    using GalaSoft.MvvmLight;

    using lilMess.Server.Models;
    using lilMess.Server.Network;

    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public class StatisticsVeiewModel : ViewModelBase
    {
        private readonly TimeSpan measureStarted = DateTime.Now.TimeOfDay;

        private readonly INetwork network;

        private StatisticsModel oldStats;

        private EnumerableDataSource<StatisticsModel> chartDataSource;
        
        private int skip;
      
        public StatisticsVeiewModel(INetwork network)
        {
            this.network = network;

            this.Collection = new DispatchingObservableCollection<StatisticsModel>();

            this.measureStarted = DateTime.Now.TimeOfDay;

            var dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            dispatcherTimer.Tick += (sender, e) => this.GetStatistics();
            dispatcherTimer.Start();
        }

        public DispatchingObservableCollection<StatisticsModel> Collection { get; set; }

        public EnumerableDataSource<StatisticsModel> ChartDataSource
        {
            get { return this.chartDataSource; }
            set { this.Set("ChartDataSource", ref this.chartDataSource, value); }
        }

        private void GetStatistics()
        {
            var stats = Mapper.Map<StatisticsModel>(this.network.StatisticsModel);

            if ((stats.Time - this.measureStarted).TotalSeconds > 30) { this.skip++; }

            if (this.oldStats != null)
            {
                this.Collection.Add(stats - this.oldStats);

                this.ChartDataSource = new EnumerableDataSource<StatisticsModel>(this.Collection.Skip(this.skip));

                this.ChartDataSource.SetXMapping(x => x.Time.TotalSeconds);
                this.ChartDataSource.SetYMapping(y => y.OutcomingTraffic);
            }

            this.oldStats = stats;
        }
    }
}