using System;
using System.Collections;

namespace CoroutinesForWpf
{
    public abstract class CoroutineBase : IEnumerator
    {
        public object Current { get; } = null;

        public abstract bool MoveNext();

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}