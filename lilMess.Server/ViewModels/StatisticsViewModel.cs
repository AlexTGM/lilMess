namespace lilMess.Server.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;

    using AutoMapper;

    using GalaSoft.MvvmLight;

    using lilMess.Server.Models;
    using lilMess.Server.Models.StatisticProviders;
    using lilMess.Server.Network;

    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public class StatisticsVeiewModel : ViewModelBase
    {
        private EnumerableDataSource<StatisticsModel> chartDataSource;

        private ChartProvider chartProvider;

        private double started, updated, timeFrom, timeTo;

        private bool lowerLimitsBounded = true, upperLimitsBounded = true;

        protected PerformanceCounter cpuCounter = new PerformanceCounter();
        protected PerformanceCounter ramCounter = new PerformanceCounter(); 

        public StatisticsVeiewModel(INetwork network)
        {
            ChartProviders = new ObservableCollection<ChartProvider>();
            Collection = new ObservableCollection<StatisticsModel>();

            ChartProviders.Add(new IncomingTrafficProvider());
            ChartProviders.Add(new OutcomingTrafficProvider());

            network.StatisticsService.Statistics += GetStatistics;

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes"); 

            StartedOn = DateTime.Now.TimeOfDay.TotalSeconds;
        }

        public ObservableCollection<StatisticsModel> Collection { get; set; }

        public ObservableCollection<ChartProvider> ChartProviders { get; set; }

        public EnumerableDataSource<StatisticsModel> ChartDataSource
        {
            get { return chartDataSource; }
            set { Set("ChartDataSource", ref chartDataSource, value); }
        }

        public ChartProvider ChartProvider
        {
            get { return chartProvider ?? ChartProviders[0]; }
            set { Set("ChartProvider", ref chartProvider, value); }
        }

        public double StartedOn
        {
            get { return Math.Round(started); }
            set
            {
                Set("StartedOn", ref started, value);
                UpdateChartDataSource();
                if (lowerLimitsBounded) TimeFrom = value;
            }
        }

        public double UpdatedOn
        {
            get { return Math.Round(updated); }
            set
            {
                Set("UpdatedOn", ref updated, value);
                UpdateChartDataSource();
                if (upperLimitsBounded) TimeTo = value;
            }
        }
        
        public double TimeFrom
        {
            get { return lowerLimitsBounded ? StartedOn : Math.Round(timeFrom); }
            set
            {
                lowerLimitsBounded = Math.Abs(Math.Round(value) - (int)StartedOn) < 0.0001;
                Set("TimeFrom", ref timeFrom, value);
            }
        }

        public double TimeTo
        {
            get { return upperLimitsBounded ? UpdatedOn : Math.Round(timeTo); }
            set
            {
                upperLimitsBounded = Math.Abs(Math.Round(value) - UpdatedOn) < 0.0001;
                Set("TimeTo", ref timeTo, value);
            }
        }

        private void GetStatistics(Network.Models.StatisticsModel statisticsModel)
        {
            var stats = Mapper.Map<StatisticsModel>(statisticsModel);
            System.Windows.Application.Current.Dispatcher.Invoke(() => Collection.Add(stats));

            stats.CpuLoad = cpuCounter.NextValue();
            stats.MemLoad = ramCounter.NextValue();

            UpdatedOn = DateTime.Now.TimeOfDay.TotalSeconds;
        }

        private void UpdateChartDataSource()
        {
            int skip, take;

            if (upperLimitsBounded && lowerLimitsBounded)
            {
                skip = (int)(UpdatedOn - StartedOn - 30);
                take = (int)(UpdatedOn - StartedOn);
            }
            else
            {
                skip = (int)(TimeFrom - StartedOn);
                take = (int)(TimeTo - TimeFrom);
            }

            var collection = Collection.Skip(skip > 0 ? skip : 0).Take(take);
            ChartDataSource = ChartProvider.GeneratePlotData(collection);
        }
    }
}