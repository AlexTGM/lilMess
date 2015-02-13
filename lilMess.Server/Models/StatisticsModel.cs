namespace lilMess.Server.Models
{
    using GalaSoft.MvvmLight;

    public class StatisticsModel : ObservableObject
    {
        private long incomingTraffic;
        private long outcomingTraffic;

        public long IncomingTraffic
        {
            get { return this.incomingTraffic; }
            set { this.Set("IncomingTraffic", ref this.incomingTraffic, value); }
        }

        public long OutcomingTraffic
        {
            get { return this.outcomingTraffic; }
            set { this.Set("OutcomingTraffic", ref this.outcomingTraffic, value); }
        }
    }
}