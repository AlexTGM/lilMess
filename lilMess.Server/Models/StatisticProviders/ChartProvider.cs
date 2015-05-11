namespace lilMess.Server.Models.StatisticProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public abstract class ChartProvider
    {
        protected ChartProvider(string horizontalAxisTitle, string verticalAxisTitle, string chartCaption, string chartHeader = null)
        {
            ChartHeader = chartHeader ?? "Статистика сервера";

            HorizontalAxisTitle = horizontalAxisTitle;
            VerticalAxisTitle = verticalAxisTitle;
            ChartCaption = chartCaption;
        }

        public string ChartHeader { get; set; }

        public string HorizontalAxisTitle { get; set; }

        public string VerticalAxisTitle { get; set; }

        public string ChartCaption { get; set; }

        public EnumerableDataSource<StatisticsModel> GeneratePlotData(IEnumerable<StatisticsModel> collection, int skip)
        {
            return SetBindings(collection.Skip(skip).AsDataSource());
        }
        public EnumerableDataSource<StatisticsModel> GeneratePlotData(IEnumerable<StatisticsModel> collection)
        {
            return SetBindings(collection.AsDataSource());
        }

        protected abstract EnumerableDataSource<StatisticsModel> SetBindings(EnumerableDataSource<StatisticsModel> dataSource);
    }
}