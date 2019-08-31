using System;
using System.Collections;

namespace CoroutinesForWpf
{
    public static class Executor
    {
        private static IPump _pump = new WpfEventPump();

        public static IDisposable StartCoroutine(IEnumerator routine)
        {
            return new Runner(_pump, routine);
        }

        public static void AssignEventPump(IPump pump)
        {
            _pump?.Dispose();
            _pump = pump;
        }

        private class Runner : IDisposable
        {
            private readonly IPump _pump;

            private Routine _routine;

            public Runner(IPump pump, IEnumerator routine)
            {
                _pump = pump;
                _routine = new Routine(routine);
                _pump.NextFrame += OnNextFrame;
            }

            public void Dispose()
            {
                _pump.NextFrame -= OnNextFrame;
            }

            private void OnNextFrame()
            {
                if (_routine.Enumerator.MoveNext())
                {
                    if (_routine.Enumerator.Current is IEnumerator e)
                    {
                        _routine = new Routine(e, _routine);
                        OnNextFrame();
                    }
                }
                else if (_routine.Parent != null)
                {
                    _routine = _routine.Parent;
                    OnNextFrame();
                }
                else
                {
                    _pump.NextFrame -= OnNextFrame;
                }
            }
        }

        private class Routine
        {
            public Routine(IEnumerator enumerator, Routine parent = null)
            {
                Enumerator = enumerator;
                Parent = parent;
            }

            public Routine Parent { get; }
            public IEnumerator Enumerator { get; }
        }
    }
}
