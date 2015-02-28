namespace lilMess.Server.Models.StatisticProviders
{
    using Microsoft.Research.DynamicDataDisplay.DataSources;

    public class IncomingTrafficProvider : ChartProvider
    {
        public IncomingTrafficProvider()
            : base("Время (секунды)", "Входящий трафик (байты)", "Входящий трафик") { }

        protected override EnumerableDataSource<StatisticsModel> SetBindings(EnumerableDataSource<StatisticsModel> dataSource)
        {
            dataSource.SetXMapping(x => x.Time.TimeOfDay.TotalSeconds);
            dataSource.SetYMapping(y => y.IncomingTraffic);

            return dataSource;
        }
    }
}