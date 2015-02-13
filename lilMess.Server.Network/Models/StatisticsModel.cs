namespace lilMess.Server.Network.Models
{
    public struct StatisticsModel
    {
        public StatisticsModel(long incoming, long outcoming)
            : this()
        {
            this.IncomingTraffic = incoming;
            this.OutcomingTraffic = outcoming;
        }

        public long IncomingTraffic { get; set; }
        public long OutcomingTraffic { get; set; }
    }
}