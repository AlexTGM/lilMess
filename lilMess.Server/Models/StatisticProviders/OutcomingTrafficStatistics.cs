namespace lilMess.Server.Models.StatisticProviders
{
    public class OutcomingTrafficProvider : StatisticsProvider
    {
        public OutcomingTrafficProvider()
            : base("Время (секунды)", "Исходящий трафик (байты)", "Исходящий трафик") { }

        protected override void SetBindings(ref Microsoft.Research.DynamicDataDisplay.DataSources.EnumerableDataSource<StatisticsModel> dataSource)
        {
            dataSource.SetXMapping(x => x.Time.TotalSeconds);
            dataSource.SetYMapping(y => y.OutcomingTraffic);
        }
    }
}