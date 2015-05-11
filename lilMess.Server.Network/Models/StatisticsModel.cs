namespace lilMess.Server.Network.Models
{
    public class StatisticsModel
    {
        public StatisticsModel() { }

        public StatisticsModel(long incoming, long outcoming)
        {
            IncomingTraffic = incoming;
            OutcomingTraffic = outcoming;
        }
        
        public long IncomingTraffic { get; set; }

        public long OutcomingTraffic { get; set; }

        public static StatisticsModel operator -(StatisticsModel obj1, StatisticsModel obj2)
        {
            var statistics = new StatisticsModel
            {
                IncomingTraffic = obj1.IncomingTraffic - obj2.IncomingTraffic,
                OutcomingTraffic = obj1.OutcomingTraffic - obj2.OutcomingTraffic
            };

            return statistics;
        }
    }
}