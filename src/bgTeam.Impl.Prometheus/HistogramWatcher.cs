using Prometheus;

namespace bgTeam.Impl.Prometheus
{
    internal class HistogramWatcher : BaseWatcher
    {
        private readonly Histogram _collector;

        public HistogramWatcher(Histogram collector, params string[] labels)
            : base(labels)
        {
            _collector = collector;
        }

        protected override void SetValue(double elapsedSeconds)
        {
            _collector
                .Labels(_labels)
                .Observe(elapsedSeconds);
        }
    }
}
