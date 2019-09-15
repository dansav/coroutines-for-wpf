using System.Collections;
using NUnit.Framework;

namespace CoroutinesDotNet.Tests
{
    public class CoroutineRunnerTests
    {
        [Test]
        public void CoroutineRunner_BeforePumpAdvances_NoFrameIsExecuted()
        {
            var pump = new TestPump();

            int i = 0;

            var runner = new CoroutineRunner(TestRoutine(), pump);

            Assert.That(i, Is.EqualTo(0));

            IEnumerator TestRoutine()
            {
                i = 1;
                yield return null;
            }
        }

        [Test]
        public void CoroutineRunner_WhenPumpAdvances_NextFrameIsExecuted()
        {
            var pump = new TestPump();

            int i = 0;

            var runner = new CoroutineRunner(TestRoutine(), pump);

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