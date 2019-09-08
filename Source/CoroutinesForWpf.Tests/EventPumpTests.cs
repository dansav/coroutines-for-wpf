using System;
using System.Collections;
using NUnit.Framework;

namespace CoroutinesForWpf.Tests
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
                Executor.AssignEventPump(pump);

                Executor.StartCoroutine(Routine());

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

        [Test]
        public void Executor_WhenAssigningNewPump_OldPumpIsDisposed()
        {
            var pump1 = new TestPump();
            var pump2 = new TestPump();

            Executor.AssignEventPump(pump1);

            Assert.That(pump1.IsDisposed, Is.False);

            Executor.AssignEventPump(pump2);

            Assert.That(pump1.IsDisposed, Is.True);
        }
    }
}