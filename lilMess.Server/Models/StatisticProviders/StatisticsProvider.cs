namespace lilMess.Server.Models.StatisticProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public abstract class StatisticsProvider
    {
        protected StatisticsProvider(string horizontalAxisTitle, string verticalAxisTitle, string statisticsCaption)
        {
            this.ChartHeader = "Статистика сервера";

            this.HorizontalAxisTitle = horizontalAxisTitle;
            this.VerticalAxisTitle = verticalAxisTitle;
            this.StatisticsCaption = statisticsCaption;
        }

        public string ChartHeader { get; set; }

        public string HorizontalAxisTitle { get; set; }

        public string VerticalAxisTitle { get; set; }

        public string StatisticsCaption { get; set; }

        public EnumerableDataSource<StatisticsModel> GeneratePlotData(IEnumerable<StatisticsModel> collection, int skip)
        {
            var result = new EnumerableDataSource<StatisticsModel>(collection.Skip(skip));

            this.SetBindings(ref result);

            return result;
        }

        protected abstract void SetBindings(ref EnumerableDataSource<StatisticsModel> dataSource);
    }
}