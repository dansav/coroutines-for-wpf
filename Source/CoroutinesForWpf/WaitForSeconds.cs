using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoroutinesForWpf
{
    public class WaitForSeconds : CoroutineBase
    {
        private readonly TimeSpan _duration;

        private DateTimeOffset _targetTime;

        public WaitForSeconds(double durationInSeconds) : this(TimeSpan.FromSeconds(durationInSeconds))
        {
        }

        public WaitForSeconds(TimeSpan duration)
        {
            _duration = duration;
        }

        public override bool MoveNext()
        {
            if (_targetTime == DateTimeOffset.MinValue)
            {
                // time to start
                _targetTime = DateTimeOffset.Now.Add(_duration);
            }

            if (_targetTime > DateTimeOffset.Now) return true;

            // reset for new use
            _targetTime = DateTimeOffset.MinValue;
            return false;
        }
    }
}
