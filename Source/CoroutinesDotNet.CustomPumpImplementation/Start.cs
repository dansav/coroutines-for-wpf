using System;
using System.Collections;

namespace CoroutinesDotNet.CustomPumpExample
{
    public static class Start
    {
        private static readonly IPump _pump = new BackgroundThreadEventPump(200);

        public static IDisposable Coroutine(IEnumerator routine)
        {
            return new CoroutineRunner(routine, _pump);
        }
    }
}