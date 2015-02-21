namespace lilMess.Server.Models
{
    using System;

    using GalaSoft.MvvmLight;

    public class StatisticsModel : ObservableObject
    {
        private TimeSpan time;

        private long incomingTraffic;

        private long outcomingTraffic;

        private float cpuLoad;

        private float memLoad;

        public StatisticsModel()
        {
            this.Time = DateTime.Now.TimeOfDay;
        }

        public TimeSpan Time { get { return this.time; } set { this.Set("Time", ref this.time, value); } }

        public long IncomingTraffic { get { return this.incomingTraffic; } set { this.Set("IncomingTraffic", ref this.incomingTraffic, value); } }

        public long OutcomingTraffic { get { return this.outcomingTraffic; } set { this.Set("OutcomingTraffic", ref this.outcomingTraffic, value); } }

        public float CpuLoad { get { return this.cpuLoad; } set { this.Set("CpuLoad", ref this.cpuLoad, value); } }

        public float MemLoad { get { return this.memLoad; } set { this.Set("MemLoad", ref this.memLoad, value); } }

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