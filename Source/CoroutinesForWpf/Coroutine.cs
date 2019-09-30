using System;
using System.Collections;
using CoroutinesDotNet;

namespace CoroutinesForWpf
{
    // alternative public api 1.
    public static class Coroutine
    {
        private static IPump _pump = new WpfEventPump();

        public static IDisposable Start(IEnumerator routine)
        {
            return new CoroutineRunner(routine, _pump);
        }
        public static void AssignEventPump(IPump pump)
        {
            _pump?.Dispose();
            _pump = pump;
        }
    }

    // alternative public api 2.
    [Obsolete("Use Coroutine.Start instead")]
    public static class Start
    {
        public static IDisposable Coroutine(IEnumerator routine)
        {
            return CoroutinesForWpf.Coroutine.Start(routine);
        }
    }

    // for backwards compatibility
    [Obsolete("Use Coroutine.Start instead")]
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