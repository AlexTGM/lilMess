namespace lilMess.Server.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Threading;

    using AutoMapper;

    using GalaSoft.MvvmLight;

    using lilMess.Server.Models;
    using lilMess.Server.Models.StatisticProviders;
    using lilMess.Server.Network;

    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public class StatisticsVeiewModel : ViewModelBase
    {
        private readonly TimeSpan measureStarted = DateTime.Now.TimeOfDay;

        private readonly INetwork network;

        private EnumerableDataSource<StatisticsModel> chartDataSource;

        private ObservableCollection<StatisticsProvider> statisticsProviders;

        private StatisticsProvider statisticsProvider;

        private StatisticsModel oldStats;

        private int skip;

        public StatisticsVeiewModel(INetwork network)
        {
            this.StatisticsProviders = new ObservableCollection<StatisticsProvider> { new IncomingTrafficProvider(), new OutcomingTrafficProvider() };

            this.network = network;

            this.Collection = new ObservableCollection<StatisticsModel>();

            var dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            dispatcherTimer.Tick += (sender, e) => this.GetStatistics();
            dispatcherTimer.Start();
        }

        public ObservableCollection<StatisticsModel> Collection { get; set; }

        public EnumerableDataSource<StatisticsModel> ChartDataSource
        {
            get { return this.chartDataSource; }
            set { this.Set("ChartDataSource", ref this.chartDataSource, value); }
        }

        public StatisticsProvider StatisticsProvider
        {
            get { return this.statisticsProvider ?? this.StatisticsProviders[0]; }
            set { this.Set("StatisticsInfo", ref this.statisticsProvider, value); }
        }

        public ObservableCollection<StatisticsProvider> StatisticsProviders
        {
            get { return this.statisticsProviders; }
            set { this.Set("StatisticsProviders", ref this.statisticsProviders, value); }
        }

        private void GetStatistics()
        {
            var stats = Mapper.Map<StatisticsModel>(this.network.StatisticsModel);

            if ((stats.Time - this.measureStarted).TotalSeconds > 30) { this.skip++; }

            if (this.oldStats != null) { this.Collection.Add(stats - this.oldStats); }

            this.ChartDataSource = StatisticsProvider.GeneratePlotData(this.Collection, this.skip);

            this.oldStats = stats;
        }
    }
}