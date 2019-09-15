using System;

namespace CoroutinesDotNet
{
    public class WaitUntil : CoroutineBase
    {
        private readonly Func<bool> _predicate;

        public WaitUntil(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public override bool MoveNext()
        {
            return _predicate() == false;
        }
    }
}