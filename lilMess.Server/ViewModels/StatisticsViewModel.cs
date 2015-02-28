namespace lilMess.Server.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
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

        public StatisticsVeiewModel(INetwork network)
        {
            this.ChartProviders = new ObservableCollection<ChartProvider>();
            this.Collection = new ObservableCollection<StatisticsModel>();

            this.ChartProviders.Add(new IncomingTrafficProvider());
            this.ChartProviders.Add(new OutcomingTrafficProvider());

            network.StatisticsService.Statistics += this.GetStatistics;

            this.StartedOn = DateTime.Now.TimeOfDay.TotalSeconds;
        }

        public ObservableCollection<StatisticsModel> Collection { get; set; }

        public ObservableCollection<ChartProvider> ChartProviders { get; set; }

        public EnumerableDataSource<StatisticsModel> ChartDataSource
        {
            get { return this.chartDataSource; }
            set { this.Set("ChartDataSource", ref this.chartDataSource, value); }
        }

        public ChartProvider ChartProvider
        {
            get { return this.chartProvider ?? this.ChartProviders[0]; }
            set { this.Set("ChartProvider", ref this.chartProvider, value); }
        }

        public double StartedOn
        {
            get { return Math.Round(this.started); }
            set
            {
                this.Set("StartedOn", ref this.started, value);
                this.UpdateChartDataSource();
                if (this.lowerLimitsBounded) this.TimeFrom = value;
            }
        }

        public double UpdatedOn
        {
            get { return Math.Round(this.updated); }
            set
            {
                this.Set("UpdatedOn", ref this.updated, value);
                this.UpdateChartDataSource();
                if (this.upperLimitsBounded) this.TimeTo = value;
            }
        }


        public double TimeFrom
        {
            get { return this.lowerLimitsBounded ? this.StartedOn : Math.Round(this.timeFrom); }
            set
            {
                this.lowerLimitsBounded = Math.Abs(Math.Round(value) - (int)this.StartedOn) < 0.0001;
                this.Set("TimeFrom", ref this.timeFrom, value);
            }
        }

        public double TimeTo
        {
            get { return this.upperLimitsBounded ? this.UpdatedOn : Math.Round(this.timeTo); }
            set
            {
                this.upperLimitsBounded = Math.Abs(Math.Round(value) - this.UpdatedOn) < 0.0001;
                this.Set("TimeTo", ref this.timeTo, value);
            }
        }

        private void GetStatistics(Network.Models.StatisticsModel statisticsModel)
        {
            var stats = Mapper.Map<StatisticsModel>(statisticsModel);
            System.Windows.Application.Current.Dispatcher.Invoke(() => this.Collection.Add(stats));

            this.UpdatedOn = DateTime.Now.TimeOfDay.TotalSeconds;
        }

        private void UpdateChartDataSource()
        {
            int skip, take;

            if (this.upperLimitsBounded && this.lowerLimitsBounded)
            {
                skip = (int)(this.UpdatedOn - this.StartedOn - 30);
                take = (int)(this.UpdatedOn - this.StartedOn);
            }
            else
            {
                skip = (int)(this.TimeFrom - this.StartedOn);
                take = (int)(this.TimeTo - this.TimeFrom);
            }

            var collection = this.Collection.Skip(skip > 0 ? skip : 0).Take(take);
            this.ChartDataSource = this.ChartProvider.GeneratePlotData(collection);
        }
    }
}