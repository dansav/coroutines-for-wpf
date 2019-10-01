using System;
using System.Collections;

namespace CoroutinesDotNet.CustomPumpExample
{
    public static class CustomCoroutine
    {
        private static readonly IPump _pump = new BackgroundThreadEventPump(200);

        public static IDisposable Start(IEnumerator routine)
        {
            return new CoroutineRunner(routine, _pump);
        }
    }
}