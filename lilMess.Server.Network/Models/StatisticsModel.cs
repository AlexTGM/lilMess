namespace lilMess.Server.Network.Models
{
    using System;

    public struct StatisticsModel
    {
        public StatisticsModel(long incoming, long outcoming)
            : this()
        {
            this.IncomingTraffic = incoming;
            this.OutcomingTraffic = outcoming;
        }

        public long IncomingTraffic { get; private set; }

        public long OutcomingTraffic { get; private set; }
    }
}