using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoroutinesDotNet
{
    public class WaitForSeconds : WaitForTimeBase
    {
        public WaitForSeconds(double durationInSeconds) : base(TimeSpan.FromSeconds(durationInSeconds))
        {
        }
    }
}
