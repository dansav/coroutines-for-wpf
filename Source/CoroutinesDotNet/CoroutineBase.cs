using System;
using System.Collections;

namespace CoroutinesDotNet
{
    public abstract class CoroutineBase : IEnumerator
    {
        public virtual object Current { get; } = null;

        public abstract bool MoveNext();

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}