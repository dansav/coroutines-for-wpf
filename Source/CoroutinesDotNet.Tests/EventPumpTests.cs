using System;
using System.Collections;
using NUnit.Framework;

namespace CoroutinesDotNet.Tests
{
    public class EventPumpTests
    {
        /// <summary>
        /// This test just shows that it is the responsibility of the pump to handle Exceptions thrown in a coroutine
        /// (to mimic the behavior of coroutines in Unity)
        /// </summary>
        [Test]
        public void PumpMustHandlleExceptionsInCoroutine()
        {
            using (var pump = new TestPump())
            {
                var runner = new CoroutineRunner(Routine(), pump);

                // frame 1
                Assert.DoesNotThrow(() => pump.Advance());

                // frame 2
                Assert.Throws<NotImplementedException>(() => pump.Advance());
            }

            IEnumerator Routine()
            {
                yield return null;
                throw new NotImplementedException("Second frame");
            }
        }
    }
}