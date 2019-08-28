using System;

namespace CoroutinesForWpf
{
    public class WaitWhile : CoroutineBase
    {
        private readonly Func<bool> _predicate;

        public WaitWhile(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public override bool MoveNext()
        {
            return _predicate();
        }
    }
}