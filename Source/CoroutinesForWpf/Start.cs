using System;
using System.Collections;
using CoroutinesDotNet;

namespace CoroutinesForWpf
{
    public static class Start
    {
        private static IPump _pump = new WpfEventPump();

        public static IDisposable Coroutine(IEnumerator routine)
        {
            return new CoroutineRunner(routine, _pump);
        }

        public static void AssignEventPump(IPump pump)
        {
            _pump?.Dispose();
            _pump = pump;
        }
    }

    // alternative public api 1.
    public static class Coroutine
    {
        public static IDisposable Start(IEnumerator routine)
        {
            return CoroutinesForWpf.Start.Coroutine(routine);
        }
    }

    // alternative public api 2.
    public static class Wpf
    {
        public static IDisposable StartCoroutine(IEnumerator routine)
        {
            return Start.Coroutine(routine);
        }
    }

    // for backwards compatibility
    public static class Executor
    {
        [Obsolete]
        public static IDisposable StartCoroutine(IEnumerator routine)
        {
            return Start.Coroutine(routine);
        }
    }

    public static class EnumeratorExtensions
    {
        public static IDisposable Start(this IEnumerator routine)
        {
            return CoroutinesForWpf.Start.Coroutine(routine);
        }
    }
}