using CoroutinesForWpf;
using NUnit.Framework;

namespace CoroutinesDotNet.Tests
{
    public class CoroutineTests
    {
        [Test]
        public void Coroutine_WhenAssigningNewPump_OldPumpIsDisposed()
        {
            var pump1 = new TestPump();
            var pump2 = new TestPump();

            Coroutine.AssignEventPump(pump1);

            Assert.That(pump1.IsDisposed, Is.False);

            Coroutine.AssignEventPump(pump2);

            Assert.That(pump1.IsDisposed, Is.True);
        }
    }
}