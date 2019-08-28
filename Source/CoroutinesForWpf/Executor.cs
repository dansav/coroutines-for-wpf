using System;
using System.Collections;
using System.Windows.Media;

namespace CoroutinesForWpf
{
    public static class Executor
    {
        public static IDisposable StartCoroutine(IEnumerator routine)
        {
            return new Runner(routine);
        }

        private class Runner : IDisposable
        {
            private Routine _routine;

            public Runner(IEnumerator routine)
            {
                _routine = new Routine(routine);
                CompositionTarget.Rendering += Pump;
            }

            public void Dispose()
            {
                CompositionTarget.Rendering -= Pump;
            }

            private void Pump(object sender, EventArgs eventArgs)
            {
                if (_routine.Value.MoveNext())
                {
                    if (_routine.Value.Current is IEnumerator e)
                    {
                        _routine = new Routine(e, _routine);
                    }
                }
                else if (_routine.Parent != null)
                {
                    _routine = _routine.Parent;
                    Pump(sender, eventArgs);
                }
                else
                {
                    CompositionTarget.Rendering -= Pump;
                }
            }
        }

        private class Routine
        {
            public Routine(IEnumerator value, Routine parent = null)
            {
                Value = value;
                Parent = parent;
            }

            public Routine Parent { get; }
            public IEnumerator Value { get; }
        }
    }
}
