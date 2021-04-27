using System;
using Prometheus;

namespace bgTeam.Prometheus.Impl
{
    public static class PrometheusExtensions
    {
        public static IDisposable MeasureDuration<T>(this T @this, params string[] labels)
            where T : Collector
        {
            switch (typeof(T))
            {
                case Type type when type == typeof(Histogram): return new HistogramWatcher(@this as Histogram, labels);
                default: throw new NotSupportedException(typeof(T).Name);
            }
        }
    }
}
