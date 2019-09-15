using System;
using System.Collections;

namespace CoroutinesForWpf
{
    internal class Explore
    {
        internal void Api()
        {
            // 1.
            var x = Coroutine.Start(DoSomething());
            x.Dispose();

            // 2. Close to the original
            var y = Start.Coroutine(DoSomething());
            y.Dispose();

            // 3. Extension method on IEnumerator (can happen regardless of the static API)
            var z = DoSomething().Start();
            z.Dispose();
        }

        private IEnumerator DoSomething()
        {
            yield return null;
        }
    }
}