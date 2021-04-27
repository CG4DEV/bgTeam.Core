using System;
using Prometheus;

namespace bgTeam.Prometheus.Impl
{
    public static class PrometheusExtensions
    {
        public static IDisposable MeasureDuration<T>(this T @this, params string[] labels)
            where T : Collector
        {
            switch (@this)
            {
                case Histogram histogram: return new HistogramWatcher(histogram, labels);
                default: throw new NotSupportedException(typeof(T).Name);
            }
        }
    }
}
