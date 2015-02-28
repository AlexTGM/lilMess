namespace lilMess.Server.Models
{
    using System;

    using GalaSoft.MvvmLight;

    public class StatisticsModel : ObservableObject
    {
        private DateTime time;

        private long incomingTraffic, outcomingTraffic;
        private float cpuLoad, memLoad;

        private StatisticsModel() { this.Time = DateTime.Now; }

        public DateTime Time
        {
            get { return this.time; }
            set { this.Set("Time", ref this.time, value); }
        }

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

        public float CpuLoad
        {
            get { return this.cpuLoad; }
            set { this.Set("CpuLoad", ref this.cpuLoad, value); }
        }

        public float MemLoad
        {
            get { return this.memLoad; }
            set { this.Set("MemLoad", ref this.memLoad, value); }
        }
    }
}