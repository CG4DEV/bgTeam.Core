using System;
using System.Diagnostics;

namespace bgTeam.Prometheus.Impl
{
    internal abstract class BaseWatcher : IDisposable
    {
        private readonly Stopwatch _sw;
        protected readonly string[] _labels;

        private bool _disposed = false;

        protected BaseWatcher(params string[] labels)
        {
            _sw = Stopwatch.StartNew();
            _labels = labels;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool flag)
        {
            if (_disposed)
            {
                return;
            }

            if (flag)
            {
                _sw.Stop();
                SetValue(_sw.Elapsed.TotalSeconds);
            }

            _disposed = true;
        }

        protected abstract void SetValue(double elapsedSeconds);
    }
}
