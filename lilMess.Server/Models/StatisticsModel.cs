namespace lilMess.Server.Models
{
    using System;

    using GalaSoft.MvvmLight;

    public class StatisticsModel : ObservableObject
    {
        private DateTime time;

        private long incomingTraffic, outcomingTraffic;
        private float cpuLoad, memLoad;

        private StatisticsModel() { Time = DateTime.Now; }

        public DateTime Time
        {
            get { return time; }
            set { Set("Time", ref time, value); }
        }

        public long IncomingTraffic
        {
            get { return incomingTraffic; }
            set { Set("IncomingTraffic", ref incomingTraffic, value); }
        }

        public long OutcomingTraffic
        {
            get { return outcomingTraffic; }
            set { Set("OutcomingTraffic", ref outcomingTraffic, value); }
        }

        public float CpuLoad
        {
            get { return cpuLoad; }
            set { Set("CpuLoad", ref cpuLoad, value); }
        }

        public float MemLoad
        {
            get { return memLoad; }
            set { Set("MemLoad", ref memLoad, value); }
        }
    }
}