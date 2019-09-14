using System;
using System.Collections;

namespace CoroutinesForWpf
{
    public class Explore
    {
        public void Api()
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


    public static class Start
    {
        public static IDisposable Coroutine(IEnumerator routine)
        {
            return CoroutinesForWpf.Coroutine.Start(routine);
        }
    }

    // for backwards compatibility
    public static class Executor
    {
        [Obsolete]
        public static IDisposable StartCoroutine(IEnumerator routine)
        {
            return Coroutine.Start(routine);
        }
    }

    public static class EnumeratorExtensions
    {
        public static IDisposable Start(this IEnumerator routine)
        {
            return Coroutine.Start(routine);
        }
    }
}