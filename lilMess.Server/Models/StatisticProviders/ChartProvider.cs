namespace lilMess.Server.Models.StatisticProviders
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public abstract class ChartProvider
    {
        protected ChartProvider(string horizontalAxisTitle, string verticalAxisTitle, string chartCaption, string chartHeader = null)
        {
            this.ChartHeader = chartHeader ?? "Статистика сервера";

            this.HorizontalAxisTitle = horizontalAxisTitle;
            this.VerticalAxisTitle = verticalAxisTitle;
            this.ChartCaption = chartCaption;
        }

        public string ChartHeader { get; set; }

        public string HorizontalAxisTitle { get; set; }

        public string VerticalAxisTitle { get; set; }

        public string ChartCaption { get; set; }

        public EnumerableDataSource<StatisticsModel> GeneratePlotData(IEnumerable<StatisticsModel> collection, int skip)
        {
            return this.SetBindings(collection.Skip(skip).AsDataSource());
        }
        public EnumerableDataSource<StatisticsModel> GeneratePlotData(IEnumerable<StatisticsModel> collection)
        {
            return this.SetBindings(collection.AsDataSource());
        }

        protected abstract EnumerableDataSource<StatisticsModel> SetBindings(EnumerableDataSource<StatisticsModel> dataSource);
    }
}