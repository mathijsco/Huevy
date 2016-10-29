using System;
using System.Threading;

namespace Huevy.Lib.Core
{
    public class AccurateThreadTiming
    {
        private readonly DateTime _waitTill;

        public static AccurateThreadTiming StartNew(TimeSpan waitTime)
        {
            return new AccurateThreadTiming(waitTime);
        }

        private AccurateThreadTiming(TimeSpan waitTime)
        {
            _waitTill = DateTime.UtcNow + waitTime;
        }

        public TimeSpan Remaining
        {
            get { return _waitTill - DateTime.UtcNow; }
        }

        public void Sleep(CancellationToken token)
        {
            TimeSpan remaining;
            while (!token.IsCancellationRequested && (remaining = this.Remaining).Ticks > 0)
            {
                Thread.Sleep((int)Math.Min(remaining.TotalMilliseconds, 500));
            }
        }
    }
}
