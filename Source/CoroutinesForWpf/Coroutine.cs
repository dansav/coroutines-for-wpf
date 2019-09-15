using System;
using System.Collections;
using CoroutinesDotNet;

namespace CoroutinesForWpf
{
    public class Coroutine
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