using System;

namespace CoroutinesDotNet
{
    public class WaitFor : WaitForTimeBase
    {
        public WaitFor(TimeSpan duration) : base(duration)
        {
        }
    }
}