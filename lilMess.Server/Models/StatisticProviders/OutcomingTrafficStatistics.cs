namespace lilMess.Server.Models.StatisticProviders
{
    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public class OutcomingTrafficProvider : ChartProvider
    {
        public OutcomingTrafficProvider()
            : base("Время (секунды)", "Исходящий трафик (байты)", "Исходящий трафик") { }

        protected override EnumerableDataSource<StatisticsModel> SetBindings(EnumerableDataSource<StatisticsModel> dataSource)
        {
            dataSource.SetXMapping(x => x.Time.TimeOfDay.TotalSeconds);
            dataSource.SetYMapping(y => y.OutcomingTraffic);

            return dataSource;
        }
    }
}