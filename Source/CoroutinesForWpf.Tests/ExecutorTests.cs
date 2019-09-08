using System.Collections;
using NUnit.Framework;

namespace CoroutinesForWpf.Tests
{
    public class ExecutorTests
    {
        [Test]
        public void Executor_BeforePumpAdvances_NoFrameIsExecuted()
        {
            var pump = new TestPump();
            Executor.AssignEventPump(pump);

            int i = 0;

            Executor.StartCoroutine(TestRoutine());

            Assert.That(i, Is.EqualTo(0));

            IEnumerator TestRoutine()
            {
                i = 1;
                yield return null;
            }
        }

        [Test]
        public void Executor_WhenPumpAdvances_NextFrameIsExecuted()
        {
            var pump = new TestPump();
            Executor.AssignEventPump(pump);

            int i = 0;

            Executor.StartCoroutine(TestRoutine());

            pump.Advance();
            Assert.That(i, Is.EqualTo(1));

            pump.Advance();
            Assert.That(i, Is.EqualTo(10));

            IEnumerator TestRoutine()
            {
                i = 1;
                yield return null;
                i = 10;
            }
        }
    }
}