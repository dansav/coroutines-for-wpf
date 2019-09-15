using System;

namespace CoroutinesDotNet
{
    public abstract class WaitForTimeBase : CoroutineBase
    {
        private readonly TimeSpan _duration;

        private DateTimeOffset _targetTime;

        protected WaitForTimeBase(TimeSpan duration)
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