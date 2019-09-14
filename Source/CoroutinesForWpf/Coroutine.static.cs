using System;
using System.Collections;

namespace CoroutinesForWpf
{
    public partial class Coroutine
    {
        private static IPump _pump = new WpfEventPump();

        public static IDisposable Start(IEnumerator routine)
        {
            return new Coroutine(routine, _pump);
        }

        public static void AssignEventPump(IPump pump)
        {
            _pump?.Dispose();
            _pump = pump;
        }
    }
}